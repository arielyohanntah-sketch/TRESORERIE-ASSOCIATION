-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : mer. 25 nov. 2026 à 19:20
-- Version du serveur : 8.0.46
-- Version de PHP : 8.3.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `tresorerie_asso`
--

DELIMITER $$
--
-- Procédures
--
DROP PROCEDURE IF EXISTS `sp_prochain_numero`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_prochain_numero` (IN `p_prefixe` VARCHAR(5), IN `p_annee` SMALLINT UNSIGNED, OUT `p_numero` VARCHAR(20))   BEGIN
  DECLARE v_num INT UNSIGNED;

  INSERT INTO sequence_piece (prefixe, annee, dernier_numero)
  VALUES (p_prefixe, p_annee, 1)
  ON DUPLICATE KEY UPDATE dernier_numero = dernier_numero + 1;

  SELECT dernier_numero INTO v_num
    FROM sequence_piece WHERE prefixe = p_prefixe AND annee = p_annee;

  SET p_numero = CONCAT(p_prefixe, '-', p_annee, '-', LPAD(v_num, 5, '0'));
END$$

DROP PROCEDURE IF EXISTS `sp_verifier_droit`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_verifier_droit` (IN `p_action` VARCHAR(20))   BEGIN
  DECLARE v_msg VARCHAR(255);
  IF @id_utilisateur_courant IS NULL THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Aucun utilisateur connecté : appelez d''abord sp_utilisateur_connecter.';
  END IF;
  IF NOT fn_a_le_droit(p_action) THEN
    SET v_msg = CONCAT('Droit insuffisant : l''action « ', p_action,
                       ' » n''est pas autorisée pour votre rôle.');
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = v_msg;
  END IF;
END$$

--
-- Fonctions
--
DROP FUNCTION IF EXISTS `fn_a_le_droit`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_a_le_droit` (`p_action` VARCHAR(20)) RETURNS TINYINT(1) READS SQL DATA BEGIN
  RETURN fn_utilisateur_a_le_droit(@id_utilisateur_courant, p_action);
END$$

DROP FUNCTION IF EXISTS `fn_exercice_de_date`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_exercice_de_date` (`p_date` DATE) RETURNS INT UNSIGNED READS SQL DATA BEGIN
  DECLARE v_id INT UNSIGNED DEFAULT NULL;
  SELECT id_exercice INTO v_id FROM exercice
   WHERE p_date BETWEEN date_debut AND date_fin LIMIT 1;
  RETURN v_id;
END$$

DROP FUNCTION IF EXISTS `fn_hash_mot_de_passe`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_hash_mot_de_passe` (`p_sel` CHAR(32), `p_mot_de_passe` VARCHAR(255)) RETURNS CHAR(64) CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci DETERMINISTIC NO SQL BEGIN
  RETURN SHA2(CONCAT(p_sel, p_mot_de_passe), 256);
END$$

DROP FUNCTION IF EXISTS `fn_solde_compte`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_solde_compte` (`p_id_compte` INT UNSIGNED, `p_date` DATE) RETURNS DECIMAL(15,2) READS SQL DATA BEGIN
  DECLARE v_initial  DECIMAL(15,2) DEFAULT 0;
  DECLARE v_mvt      DECIMAL(15,2) DEFAULT 0;
  DECLARE v_vir_in   DECIMAL(15,2) DEFAULT 0;
  DECLARE v_vir_out  DECIMAL(15,2) DEFAULT 0;

  SELECT solde_initial INTO v_initial FROM compte WHERE id_compte = p_id_compte;

  SELECT COALESCE(SUM(CASE WHEN type_mouvement = 'RECETTE' THEN montant ELSE -montant END), 0)
    INTO v_mvt
    FROM mouvement
   WHERE id_compte = p_id_compte AND statut = 'VALIDE'
     AND (p_date IS NULL OR date_mouvement <= p_date);

  SELECT COALESCE(SUM(montant), 0) INTO v_vir_in
    FROM virement
   WHERE id_compte_dest = p_id_compte AND statut = 'VALIDE'
     AND (p_date IS NULL OR date_virement <= p_date);

  SELECT COALESCE(SUM(montant), 0) INTO v_vir_out
    FROM virement
   WHERE id_compte_source = p_id_compte AND statut = 'VALIDE'
     AND (p_date IS NULL OR date_virement <= p_date);

  RETURN COALESCE(v_initial, 0) + v_mvt + v_vir_in - v_vir_out;
END$$

DROP FUNCTION IF EXISTS `fn_total_ventile`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_total_ventile` (`p_id_mouvement` INT UNSIGNED) RETURNS DECIMAL(15,2) READS SQL DATA BEGIN
  DECLARE v_total DECIMAL(15,2) DEFAULT 0;
  SELECT COALESCE(SUM(montant), 0) INTO v_total
    FROM ventilation WHERE id_mouvement = p_id_mouvement;
  RETURN v_total;
END$$

DROP FUNCTION IF EXISTS `fn_utilisateur_a_le_droit`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_utilisateur_a_le_droit` (`p_id_utilisateur` INT UNSIGNED, `p_action` VARCHAR(20)) RETURNS TINYINT(1) READS SQL DATA BEGIN
  DECLARE v_role   VARCHAR(30) DEFAULT NULL;
  DECLARE v_actif  BOOLEAN DEFAULT FALSE;
  DECLARE v_echecs INT DEFAULT 0;

  IF p_id_utilisateur IS NULL THEN
    RETURN FALSE;
  END IF;

  SELECT role, actif, tentatives_echouees INTO v_role, v_actif, v_echecs
    FROM utilisateur WHERE id_utilisateur = p_id_utilisateur;

  IF v_role IS NULL OR v_actif = FALSE OR v_echecs >= 5 THEN
    RETURN FALSE;
  END IF;

  RETURN CASE p_action
    WHEN 'SAISIR'       THEN v_role IN ('ADMIN','TRESORIER','TRESORIER_ADJOINT')
    WHEN 'VALIDER'      THEN v_role IN ('ADMIN','PRESIDENT','TRESORIER')
    WHEN 'OPERER'       THEN v_role IN ('ADMIN','PRESIDENT','TRESORIER','TRESORIER_ADJOINT')
    WHEN 'PARAMETRER'   THEN v_role IN ('ADMIN','PRESIDENT','TRESORIER')
    WHEN 'CLOTURER'     THEN v_role IN ('ADMIN','PRESIDENT')
    WHEN 'UTILISATEURS' THEN v_role = 'ADMIN'
    WHEN 'CONSULTER'    THEN TRUE
    ELSE FALSE
  END;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `budget_centre`
--

DROP TABLE IF EXISTS `budget_centre`;
CREATE TABLE IF NOT EXISTS `budget_centre` (
  `id_budget` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_centre` int UNSIGNED NOT NULL,
  `id_exercice` int UNSIGNED NOT NULL,
  `recettes_prevues` decimal(15,2) NOT NULL DEFAULT '0.00',
  `depenses_prevues` decimal(15,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id_budget`),
  UNIQUE KEY `uq_budget` (`id_centre`,`id_exercice`),
  KEY `fk_budget_exercice` (`id_exercice`)
) ;

-- --------------------------------------------------------

--
-- Structure de la table `categorie`
--

DROP TABLE IF EXISTS `categorie`;
CREATE TABLE IF NOT EXISTS `categorie` (
  `id_categorie` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `sens` enum('RECETTE','DEPENSE') COLLATE utf8mb4_unicode_ci NOT NULL,
  `actif` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id_categorie`),
  UNIQUE KEY `uq_categorie_code` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `centre_analytique`
--

DROP TABLE IF EXISTS `centre_analytique`;
CREATE TABLE IF NOT EXISTS `centre_analytique` (
  `id_centre` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `description` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `id_centre_parent` int UNSIGNED DEFAULT NULL,
  `responsable` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `actif` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id_centre`),
  UNIQUE KEY `uq_centre_code` (`code`),
  KEY `fk_centre_parent` (`id_centre_parent`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `compte`
--

DROP TABLE IF EXISTS `compte`;
CREATE TABLE IF NOT EXISTS `compte` (
  `id_compte` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `libelle` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `type_compte` enum('CAISSE','BANQUE','MOBILE_MONEY') COLLATE utf8mb4_unicode_ci NOT NULL,
  `etablissement` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `numero_compte` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `solde_initial` decimal(15,2) NOT NULL DEFAULT '0.00',
  `decouvert_autorise` decimal(15,2) NOT NULL DEFAULT '0.00',
  `date_ouverture` date NOT NULL,
  `actif` tinyint(1) NOT NULL DEFAULT '1',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_compte`),
  UNIQUE KEY `uq_compte_code` (`code`)
) ;

-- --------------------------------------------------------

--
-- Structure de la table `exercice`
--

DROP TABLE IF EXISTS `exercice`;
CREATE TABLE IF NOT EXISTS `exercice` (
  `id_exercice` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `libelle` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `date_debut` date NOT NULL,
  `date_fin` date NOT NULL,
  `statut` enum('OUVERT','CLOTURE') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'OUVERT',
  `date_cloture` datetime DEFAULT NULL,
  PRIMARY KEY (`id_exercice`),
  UNIQUE KEY `uq_exercice_libelle` (`libelle`)
) ;

-- --------------------------------------------------------

--
-- Structure de la table `journal_audit`
--

DROP TABLE IF EXISTS `journal_audit`;
CREATE TABLE IF NOT EXISTS `journal_audit` (
  `id_audit` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `table_cible` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `id_enregistrement` int UNSIGNED NOT NULL,
  `action` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL,
  `details` json DEFAULT NULL,
  `id_utilisateur` int UNSIGNED DEFAULT NULL,
  `utilisateur_sql` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `date_action` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_audit`),
  KEY `fk_audit_utilisateur` (`id_utilisateur`),
  KEY `ix_audit_cible` (`table_cible`,`id_enregistrement`),
  KEY `ix_audit_date` (`date_action`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `mouvement`
--

DROP TABLE IF EXISTS `mouvement`;
CREATE TABLE IF NOT EXISTS `mouvement` (
  `id_mouvement` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `numero_piece` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `type_mouvement` enum('RECETTE','DEPENSE') COLLATE utf8mb4_unicode_ci NOT NULL,
  `date_mouvement` date NOT NULL,
  `id_exercice` int UNSIGNED DEFAULT NULL,
  `id_compte` int UNSIGNED NOT NULL,
  `id_categorie` int UNSIGNED NOT NULL,
  `id_tiers` int UNSIGNED DEFAULT NULL,
  `montant` decimal(15,2) NOT NULL,
  `mode_paiement` enum('ESPECES','CHEQUE','VIREMENT','MOBILE_MONEY','CARTE') COLLATE utf8mb4_unicode_ci NOT NULL,
  `reference_externe` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `libelle` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `statut` enum('BROUILLON','VALIDE','ANNULE') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'BROUILLON',
  `motif_annulation` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `id_utilisateur_saisie` int UNSIGNED NOT NULL,
  `id_utilisateur_validation` int UNSIGNED DEFAULT NULL,
  `id_utilisateur_annulation` int UNSIGNED DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `validated_at` datetime DEFAULT NULL,
  `annule_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id_mouvement`),
  UNIQUE KEY `uq_mouvement_piece` (`numero_piece`),
  KEY `fk_mouvement_exercice` (`id_exercice`),
  KEY `fk_mouvement_categorie` (`id_categorie`),
  KEY `fk_mouvement_tiers` (`id_tiers`),
  KEY `fk_mouvement_saisie` (`id_utilisateur_saisie`),
  KEY `fk_mouvement_validation` (`id_utilisateur_validation`),
  KEY `fk_mouvement_annulation` (`id_utilisateur_annulation`),
  KEY `ix_mouvement_compte_date` (`id_compte`,`date_mouvement`),
  KEY `ix_mouvement_statut` (`statut`),
  KEY `ix_mouvement_date` (`date_mouvement`)
) ;

-- --------------------------------------------------------

--
-- Structure de la table `piece_justificative`
--

DROP TABLE IF EXISTS `piece_justificative`;
CREATE TABLE IF NOT EXISTS `piece_justificative` (
  `id_justificatif` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_mouvement` int UNSIGNED NOT NULL,
  `type_piece` enum('FACTURE','RECU','BON_DE_CAISSE','RELEVE','CONTRAT','AUTRE') COLLATE utf8mb4_unicode_ci NOT NULL,
  `reference` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `date_piece` date NOT NULL,
  `montant` decimal(15,2) DEFAULT NULL,
  `chemin_fichier` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `date_ajout` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_justificatif`),
  KEY `fk_justificatif_mouvement` (`id_mouvement`)
) ;

-- --------------------------------------------------------

--
-- Structure de la table `sequence_piece`
--

DROP TABLE IF EXISTS `sequence_piece`;
CREATE TABLE IF NOT EXISTS `sequence_piece` (
  `prefixe` varchar(5) COLLATE utf8mb4_unicode_ci NOT NULL,
  `annee` smallint UNSIGNED NOT NULL,
  `dernier_numero` int UNSIGNED NOT NULL DEFAULT '0',
  PRIMARY KEY (`prefixe`,`annee`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `tiers`
--

DROP TABLE IF EXISTS `tiers`;
CREATE TABLE IF NOT EXISTS `tiers` (
  `id_tiers` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `nom` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `type_tiers` enum('MEMBRE','FOURNISSEUR','DONATEUR','PARTENAIRE','AUTRE') COLLATE utf8mb4_unicode_ci NOT NULL,
  `telephone` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `email` varchar(150) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `adresse` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `actif` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id_tiers`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
CREATE TABLE IF NOT EXISTS `utilisateur` (
  `id_utilisateur` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `nom` varchar(60) COLLATE utf8mb4_unicode_ci NOT NULL,
  `prenoms` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `login` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `mot_de_passe_hash` char(64) COLLATE utf8mb4_unicode_ci NOT NULL,
  `sel` char(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  `role` enum('ADMIN','PRESIDENT','TRESORIER','TRESORIER_ADJOINT','COMMISSAIRE_COMPTES','MEMBRE_BUREAU') COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(150) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `telephone` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `actif` tinyint(1) NOT NULL DEFAULT '1',
  `tentatives_echouees` tinyint UNSIGNED NOT NULL DEFAULT '0',
  `date_creation` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `derniere_connexion` datetime DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`),
  UNIQUE KEY `uq_utilisateur_login` (`login`),
  UNIQUE KEY `uq_utilisateur_email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Structure de la table `ventilation`
--

DROP TABLE IF EXISTS `ventilation`;
CREATE TABLE IF NOT EXISTS `ventilation` (
  `id_ventilation` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_mouvement` int UNSIGNED NOT NULL,
  `id_centre` int UNSIGNED NOT NULL,
  `montant` decimal(15,2) NOT NULL,
  PRIMARY KEY (`id_ventilation`),
  UNIQUE KEY `uq_ventilation` (`id_mouvement`,`id_centre`),
  KEY `fk_ventilation_centre` (`id_centre`)
) ;

-- --------------------------------------------------------

--
-- Structure de la table `virement`
--

DROP TABLE IF EXISTS `virement`;
CREATE TABLE IF NOT EXISTS `virement` (
  `id_virement` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `numero_piece` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `date_virement` date NOT NULL,
  `id_exercice` int UNSIGNED DEFAULT NULL,
  `id_compte_source` int UNSIGNED NOT NULL,
  `id_compte_dest` int UNSIGNED NOT NULL,
  `montant` decimal(15,2) NOT NULL,
  `libelle` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `statut` enum('VALIDE','ANNULE') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'VALIDE',
  `motif_annulation` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `id_utilisateur_saisie` int UNSIGNED NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_virement`),
  UNIQUE KEY `uq_virement_piece` (`numero_piece`),
  KEY `fk_virement_exercice` (`id_exercice`),
  KEY `fk_virement_source` (`id_compte_source`),
  KEY `fk_virement_dest` (`id_compte_dest`),
  KEY `fk_virement_saisie` (`id_utilisateur_saisie`)
) ;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `budget_centre`
--
ALTER TABLE `budget_centre`
  ADD CONSTRAINT `fk_budget_centre` FOREIGN KEY (`id_centre`) REFERENCES `centre_analytique` (`id_centre`),
  ADD CONSTRAINT `fk_budget_exercice` FOREIGN KEY (`id_exercice`) REFERENCES `exercice` (`id_exercice`);

--
-- Contraintes pour la table `centre_analytique`
--
ALTER TABLE `centre_analytique`
  ADD CONSTRAINT `fk_centre_parent` FOREIGN KEY (`id_centre_parent`) REFERENCES `centre_analytique` (`id_centre`);

--
-- Contraintes pour la table `journal_audit`
--
ALTER TABLE `journal_audit`
  ADD CONSTRAINT `fk_audit_utilisateur` FOREIGN KEY (`id_utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`) ON DELETE SET NULL;

--
-- Contraintes pour la table `mouvement`
--
ALTER TABLE `mouvement`
  ADD CONSTRAINT `fk_mouvement_annulation` FOREIGN KEY (`id_utilisateur_annulation`) REFERENCES `utilisateur` (`id_utilisateur`),
  ADD CONSTRAINT `fk_mouvement_categorie` FOREIGN KEY (`id_categorie`) REFERENCES `categorie` (`id_categorie`),
  ADD CONSTRAINT `fk_mouvement_compte` FOREIGN KEY (`id_compte`) REFERENCES `compte` (`id_compte`),
  ADD CONSTRAINT `fk_mouvement_exercice` FOREIGN KEY (`id_exercice`) REFERENCES `exercice` (`id_exercice`),
  ADD CONSTRAINT `fk_mouvement_saisie` FOREIGN KEY (`id_utilisateur_saisie`) REFERENCES `utilisateur` (`id_utilisateur`),
  ADD CONSTRAINT `fk_mouvement_tiers` FOREIGN KEY (`id_tiers`) REFERENCES `tiers` (`id_tiers`),
  ADD CONSTRAINT `fk_mouvement_validation` FOREIGN KEY (`id_utilisateur_validation`) REFERENCES `utilisateur` (`id_utilisateur`);

--
-- Contraintes pour la table `piece_justificative`
--
ALTER TABLE `piece_justificative`
  ADD CONSTRAINT `fk_justificatif_mouvement` FOREIGN KEY (`id_mouvement`) REFERENCES `mouvement` (`id_mouvement`) ON DELETE CASCADE;

--
-- Contraintes pour la table `ventilation`
--
ALTER TABLE `ventilation`
  ADD CONSTRAINT `fk_ventilation_centre` FOREIGN KEY (`id_centre`) REFERENCES `centre_analytique` (`id_centre`),
  ADD CONSTRAINT `fk_ventilation_mouvement` FOREIGN KEY (`id_mouvement`) REFERENCES `mouvement` (`id_mouvement`) ON DELETE CASCADE;

--
-- Contraintes pour la table `virement`
--
ALTER TABLE `virement`
  ADD CONSTRAINT `fk_virement_dest` FOREIGN KEY (`id_compte_dest`) REFERENCES `compte` (`id_compte`),
  ADD CONSTRAINT `fk_virement_exercice` FOREIGN KEY (`id_exercice`) REFERENCES `exercice` (`id_exercice`),
  ADD CONSTRAINT `fk_virement_saisie` FOREIGN KEY (`id_utilisateur_saisie`) REFERENCES `utilisateur` (`id_utilisateur`),
  ADD CONSTRAINT `fk_virement_source` FOREIGN KEY (`id_compte_source`) REFERENCES `compte` (`id_compte`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
