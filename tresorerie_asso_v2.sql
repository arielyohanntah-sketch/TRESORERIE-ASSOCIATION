-- =====================================================================
--  Base de données : `tresorerie_asso`
--  Script complet de (re)création — version corrigée du 25/11/2026
--  (MySQL 8.0.16 ou plus récent)
--
--  ⚠ ATTENTION : ce script SUPPRIME puis RECRÉE toutes les tables.
--    Toutes les données existantes de la base seront effacées.
--
--  Corrections par rapport à l'export phpMyAdmin :
--    - désactivation des clés étrangères pendant la création (erreur #3730)
--    - ENGINE / CHARSET / COLLATE ajoutés sur les tables qui les avaient perdus
--    - contraintes CHECK restaurées (montants positifs, dates cohérentes...)
--    - procédure manquante sp_utilisateur_connecter ajoutée
--    - procédures sp_utilisateur_creer / deconnecter / debloquer ajoutées
--    - triggers de contrôle, de numérotation et d'audit ajoutés
--    - vue v_solde_compte ajoutée
--    - DEFINER retirés (le script fonctionne avec n'importe quel compte)
-- =====================================================================

SET NAMES utf8mb4;
SET time_zone = '+00:00';
SET FOREIGN_KEY_CHECKS = 0;

CREATE DATABASE IF NOT EXISTS `tresorerie_asso`
  DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `tresorerie_asso`;

-- ---------------------------------------------------------------------
-- Suppression des anciens objets
-- ---------------------------------------------------------------------
DROP VIEW IF EXISTS `v_solde_compte`;

DROP TABLE IF EXISTS `budget_centre`, `categorie`, `centre_analytique`, `compte`,
  `exercice`, `journal_audit`, `mouvement`, `piece_justificative`,
  `sequence_piece`, `tiers`, `utilisateur`, `ventilation`, `virement`;

DROP PROCEDURE IF EXISTS `sp_prochain_numero`;
DROP PROCEDURE IF EXISTS `sp_verifier_droit`;
DROP PROCEDURE IF EXISTS `sp_utilisateur_creer`;
DROP PROCEDURE IF EXISTS `sp_utilisateur_connecter`;
DROP PROCEDURE IF EXISTS `sp_utilisateur_deconnecter`;
DROP PROCEDURE IF EXISTS `sp_utilisateur_debloquer`;
DROP PROCEDURE IF EXISTS `sp_controler_exercice`;
DROP PROCEDURE IF EXISTS `sp_controler_solde`;
DROP FUNCTION IF EXISTS `fn_a_le_droit`;
DROP FUNCTION IF EXISTS `fn_exercice_de_date`;
DROP FUNCTION IF EXISTS `fn_hash_mot_de_passe`;
DROP FUNCTION IF EXISTS `fn_solde_compte`;
DROP FUNCTION IF EXISTS `fn_total_ventile`;
DROP FUNCTION IF EXISTS `fn_utilisateur_a_le_droit`;

-- =====================================================================
-- TABLES
-- =====================================================================

-- ---------------------------------------------------------------------
-- utilisateur
-- ---------------------------------------------------------------------
CREATE TABLE `utilisateur` (
  `id_utilisateur`      int UNSIGNED NOT NULL AUTO_INCREMENT,
  `nom`                 varchar(60)  NOT NULL,
  `prenoms`             varchar(100) NOT NULL,
  `login`               varchar(50)  NOT NULL,
  `mot_de_passe_hash`   char(64)     NOT NULL,
  `sel`                 char(32)     NOT NULL,
  `role` enum('ADMIN','PRESIDENT','TRESORIER','TRESORIER_ADJOINT',
              'COMMISSAIRE_COMPTES','MEMBRE_BUREAU') NOT NULL,
  `email`               varchar(150) DEFAULT NULL,
  `telephone`           varchar(30)  DEFAULT NULL,
  `actif`               tinyint(1)   NOT NULL DEFAULT 1,
  `tentatives_echouees` tinyint UNSIGNED NOT NULL DEFAULT 0,
  `date_creation`       datetime     NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `derniere_connexion`  datetime     DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`),
  UNIQUE KEY `uq_utilisateur_login` (`login`),
  UNIQUE KEY `uq_utilisateur_email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- exercice
-- ---------------------------------------------------------------------
CREATE TABLE `exercice` (
  `id_exercice`  int UNSIGNED NOT NULL AUTO_INCREMENT,
  `libelle`      varchar(50) NOT NULL,
  `date_debut`   date NOT NULL,
  `date_fin`     date NOT NULL,
  `statut`       enum('OUVERT','CLOTURE') NOT NULL DEFAULT 'OUVERT',
  `date_cloture` datetime DEFAULT NULL,
  PRIMARY KEY (`id_exercice`),
  UNIQUE KEY `uq_exercice_libelle` (`libelle`),
  CONSTRAINT `chk_exercice_dates` CHECK (`date_fin` > `date_debut`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- compte (caisse, banque, mobile money)
-- ---------------------------------------------------------------------
CREATE TABLE `compte` (
  `id_compte`          int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code`               varchar(10)  NOT NULL,
  `libelle`            varchar(100) NOT NULL,
  `type_compte`        enum('CAISSE','BANQUE','MOBILE_MONEY') NOT NULL,
  `etablissement`      varchar(100) DEFAULT NULL,
  `numero_compte`      varchar(50)  DEFAULT NULL,
  `solde_initial`      decimal(15,2) NOT NULL DEFAULT 0.00,
  `decouvert_autorise` decimal(15,2) NOT NULL DEFAULT 0.00,
  `date_ouverture`     date NOT NULL,
  `actif`              tinyint(1) NOT NULL DEFAULT 1,
  `created_at`         datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_compte`),
  UNIQUE KEY `uq_compte_code` (`code`),
  CONSTRAINT `chk_compte_decouvert` CHECK (`decouvert_autorise` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- categorie (nature de recette / dépense)
-- ---------------------------------------------------------------------
CREATE TABLE `categorie` (
  `id_categorie` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code`         varchar(10)  NOT NULL,
  `libelle`      varchar(100) NOT NULL,
  `sens`         enum('RECETTE','DEPENSE') NOT NULL,
  `actif`        tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_categorie`),
  UNIQUE KEY `uq_categorie_code` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- tiers (membres, fournisseurs, donateurs...)
-- ---------------------------------------------------------------------
CREATE TABLE `tiers` (
  `id_tiers`   int UNSIGNED NOT NULL AUTO_INCREMENT,
  `nom`        varchar(150) NOT NULL,
  `type_tiers` enum('MEMBRE','FOURNISSEUR','DONATEUR','PARTENAIRE','AUTRE') NOT NULL,
  `telephone`  varchar(30)  DEFAULT NULL,
  `email`      varchar(150) DEFAULT NULL,
  `adresse`    varchar(255) DEFAULT NULL,
  `actif`      tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_tiers`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- centre_analytique (projets, activités...)
-- ---------------------------------------------------------------------
CREATE TABLE `centre_analytique` (
  `id_centre`        int UNSIGNED NOT NULL AUTO_INCREMENT,
  `code`             varchar(10)  NOT NULL,
  `libelle`          varchar(100) NOT NULL,
  `description`      varchar(255) DEFAULT NULL,
  `id_centre_parent` int UNSIGNED DEFAULT NULL,
  `responsable`      varchar(100) DEFAULT NULL,
  `actif`            tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_centre`),
  UNIQUE KEY `uq_centre_code` (`code`),
  KEY `fk_centre_parent` (`id_centre_parent`),
  CONSTRAINT `fk_centre_parent` FOREIGN KEY (`id_centre_parent`)
    REFERENCES `centre_analytique` (`id_centre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- budget_centre (budget prévisionnel par centre et par exercice)
-- ---------------------------------------------------------------------
CREATE TABLE `budget_centre` (
  `id_budget`        int UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_centre`        int UNSIGNED NOT NULL,
  `id_exercice`      int UNSIGNED NOT NULL,
  `recettes_prevues` decimal(15,2) NOT NULL DEFAULT 0.00,
  `depenses_prevues` decimal(15,2) NOT NULL DEFAULT 0.00,
  PRIMARY KEY (`id_budget`),
  UNIQUE KEY `uq_budget` (`id_centre`,`id_exercice`),
  KEY `fk_budget_exercice` (`id_exercice`),
  CONSTRAINT `fk_budget_centre`   FOREIGN KEY (`id_centre`)   REFERENCES `centre_analytique` (`id_centre`),
  CONSTRAINT `fk_budget_exercice` FOREIGN KEY (`id_exercice`) REFERENCES `exercice` (`id_exercice`),
  CONSTRAINT `chk_budget_recettes` CHECK (`recettes_prevues` >= 0),
  CONSTRAINT `chk_budget_depenses` CHECK (`depenses_prevues` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- mouvement (recettes et dépenses)
-- ---------------------------------------------------------------------
CREATE TABLE `mouvement` (
  `id_mouvement`              int UNSIGNED NOT NULL AUTO_INCREMENT,
  `numero_piece`              varchar(20) DEFAULT NULL,
  `type_mouvement`            enum('RECETTE','DEPENSE') NOT NULL,
  `date_mouvement`            date NOT NULL,
  `id_exercice`               int UNSIGNED DEFAULT NULL,
  `id_compte`                 int UNSIGNED NOT NULL,
  `id_categorie`              int UNSIGNED NOT NULL,
  `id_tiers`                  int UNSIGNED DEFAULT NULL,
  `montant`                   decimal(15,2) NOT NULL,
  `mode_paiement`             enum('ESPECES','CHEQUE','VIREMENT','MOBILE_MONEY','CARTE') NOT NULL,
  `reference_externe`         varchar(50)  DEFAULT NULL,
  `libelle`                   varchar(255) NOT NULL,
  `statut`                    enum('BROUILLON','VALIDE','ANNULE') NOT NULL DEFAULT 'BROUILLON',
  `motif_annulation`          varchar(255) DEFAULT NULL,
  `id_utilisateur_saisie`     int UNSIGNED NOT NULL,
  `id_utilisateur_validation` int UNSIGNED DEFAULT NULL,
  `id_utilisateur_annulation` int UNSIGNED DEFAULT NULL,
  `created_at`                datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `validated_at`              datetime DEFAULT NULL,
  `annule_at`                 datetime DEFAULT NULL,
  PRIMARY KEY (`id_mouvement`),
  UNIQUE KEY `uq_mouvement_piece` (`numero_piece`),
  KEY `fk_mouvement_exercice`    (`id_exercice`),
  KEY `fk_mouvement_categorie`   (`id_categorie`),
  KEY `fk_mouvement_tiers`       (`id_tiers`),
  KEY `fk_mouvement_saisie`      (`id_utilisateur_saisie`),
  KEY `fk_mouvement_validation`  (`id_utilisateur_validation`),
  KEY `fk_mouvement_annulation`  (`id_utilisateur_annulation`),
  KEY `ix_mouvement_compte_date` (`id_compte`,`date_mouvement`),
  KEY `ix_mouvement_statut`      (`statut`),
  KEY `ix_mouvement_date`        (`date_mouvement`),
  CONSTRAINT `fk_mouvement_annulation` FOREIGN KEY (`id_utilisateur_annulation`) REFERENCES `utilisateur` (`id_utilisateur`),
  CONSTRAINT `fk_mouvement_categorie`  FOREIGN KEY (`id_categorie`)              REFERENCES `categorie` (`id_categorie`),
  CONSTRAINT `fk_mouvement_compte`     FOREIGN KEY (`id_compte`)                 REFERENCES `compte` (`id_compte`),
  CONSTRAINT `fk_mouvement_exercice`   FOREIGN KEY (`id_exercice`)               REFERENCES `exercice` (`id_exercice`),
  CONSTRAINT `fk_mouvement_saisie`     FOREIGN KEY (`id_utilisateur_saisie`)     REFERENCES `utilisateur` (`id_utilisateur`),
  CONSTRAINT `fk_mouvement_tiers`      FOREIGN KEY (`id_tiers`)                  REFERENCES `tiers` (`id_tiers`),
  CONSTRAINT `fk_mouvement_validation` FOREIGN KEY (`id_utilisateur_validation`) REFERENCES `utilisateur` (`id_utilisateur`),
  CONSTRAINT `chk_mouvement_montant`   CHECK (`montant` > 0),
  CONSTRAINT `chk_mouvement_annulation` CHECK (`statut` <> 'ANNULE' OR `motif_annulation` IS NOT NULL)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- piece_justificative (factures, reçus...)
-- ---------------------------------------------------------------------
CREATE TABLE `piece_justificative` (
  `id_justificatif` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_mouvement`    int UNSIGNED NOT NULL,
  `type_piece`      enum('FACTURE','RECU','BON_DE_CAISSE','RELEVE','CONTRAT','AUTRE') NOT NULL,
  `reference`       varchar(50)  DEFAULT NULL,
  `date_piece`      date NOT NULL,
  `montant`         decimal(15,2) DEFAULT NULL,
  `chemin_fichier`  varchar(255) DEFAULT NULL,
  `date_ajout`      datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_justificatif`),
  KEY `fk_justificatif_mouvement` (`id_mouvement`),
  CONSTRAINT `fk_justificatif_mouvement` FOREIGN KEY (`id_mouvement`)
    REFERENCES `mouvement` (`id_mouvement`) ON DELETE CASCADE,
  CONSTRAINT `chk_justificatif_montant` CHECK (`montant` IS NULL OR `montant` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- ventilation (répartition d'un mouvement sur des centres analytiques)
-- ---------------------------------------------------------------------
CREATE TABLE `ventilation` (
  `id_ventilation` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_mouvement`   int UNSIGNED NOT NULL,
  `id_centre`      int UNSIGNED NOT NULL,
  `montant`        decimal(15,2) NOT NULL,
  PRIMARY KEY (`id_ventilation`),
  UNIQUE KEY `uq_ventilation` (`id_mouvement`,`id_centre`),
  KEY `fk_ventilation_centre` (`id_centre`),
  CONSTRAINT `fk_ventilation_centre`    FOREIGN KEY (`id_centre`)    REFERENCES `centre_analytique` (`id_centre`),
  CONSTRAINT `fk_ventilation_mouvement` FOREIGN KEY (`id_mouvement`) REFERENCES `mouvement` (`id_mouvement`) ON DELETE CASCADE,
  CONSTRAINT `chk_ventilation_montant`  CHECK (`montant` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- virement (transfert entre deux comptes de l'association)
-- ---------------------------------------------------------------------
CREATE TABLE `virement` (
  `id_virement`           int UNSIGNED NOT NULL AUTO_INCREMENT,
  `numero_piece`          varchar(20) DEFAULT NULL,
  `date_virement`         date NOT NULL,
  `id_exercice`           int UNSIGNED DEFAULT NULL,
  `id_compte_source`      int UNSIGNED NOT NULL,
  `id_compte_dest`        int UNSIGNED NOT NULL,
  `montant`               decimal(15,2) NOT NULL,
  `libelle`               varchar(255) NOT NULL,
  `statut`                enum('VALIDE','ANNULE') NOT NULL DEFAULT 'VALIDE',
  `motif_annulation`      varchar(255) DEFAULT NULL,
  `id_utilisateur_saisie` int UNSIGNED NOT NULL,
  `created_at`            datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_virement`),
  UNIQUE KEY `uq_virement_piece` (`numero_piece`),
  KEY `fk_virement_exercice` (`id_exercice`),
  KEY `fk_virement_source`   (`id_compte_source`),
  KEY `fk_virement_dest`     (`id_compte_dest`),
  KEY `fk_virement_saisie`   (`id_utilisateur_saisie`),
  CONSTRAINT `fk_virement_dest`     FOREIGN KEY (`id_compte_dest`)        REFERENCES `compte` (`id_compte`),
  CONSTRAINT `fk_virement_exercice` FOREIGN KEY (`id_exercice`)           REFERENCES `exercice` (`id_exercice`),
  CONSTRAINT `fk_virement_saisie`   FOREIGN KEY (`id_utilisateur_saisie`) REFERENCES `utilisateur` (`id_utilisateur`),
  CONSTRAINT `fk_virement_source`   FOREIGN KEY (`id_compte_source`)      REFERENCES `compte` (`id_compte`),
  CONSTRAINT `chk_virement_montant` CHECK (`montant` > 0),
  CONSTRAINT `chk_virement_comptes` CHECK (`id_compte_source` <> `id_compte_dest`),
  CONSTRAINT `chk_virement_annulation` CHECK (`statut` <> 'ANNULE' OR `motif_annulation` IS NOT NULL)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- sequence_piece (compteur des numéros de pièce par préfixe et année)
-- ---------------------------------------------------------------------
CREATE TABLE `sequence_piece` (
  `prefixe`        varchar(5) NOT NULL,
  `annee`          smallint UNSIGNED NOT NULL,
  `dernier_numero` int UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`prefixe`,`annee`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ---------------------------------------------------------------------
-- journal_audit (traçabilité des opérations)
-- ---------------------------------------------------------------------
CREATE TABLE `journal_audit` (
  `id_audit`          bigint UNSIGNED NOT NULL AUTO_INCREMENT,
  `table_cible`       varchar(50) NOT NULL,
  `id_enregistrement` int UNSIGNED NOT NULL,
  `action`            varchar(20) NOT NULL,
  `details`           json DEFAULT NULL,
  `id_utilisateur`    int UNSIGNED DEFAULT NULL,
  `utilisateur_sql`   varchar(100) NOT NULL,
  `date_action`       datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_audit`),
  KEY `fk_audit_utilisateur` (`id_utilisateur`),
  KEY `ix_audit_cible` (`table_cible`,`id_enregistrement`),
  KEY `ix_audit_date`  (`date_action`),
  CONSTRAINT `fk_audit_utilisateur` FOREIGN KEY (`id_utilisateur`)
    REFERENCES `utilisateur` (`id_utilisateur`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =====================================================================
-- FONCTIONS ET PROCÉDURES
-- =====================================================================
DELIMITER $$

-- Hachage du mot de passe (SHA-256 salé)
CREATE FUNCTION `fn_hash_mot_de_passe` (`p_sel` CHAR(32), `p_mot_de_passe` VARCHAR(255))
RETURNS CHAR(64) CHARSET utf8mb4 COLLATE utf8mb4_unicode_ci
DETERMINISTIC NO SQL
BEGIN
  RETURN SHA2(CONCAT(p_sel, p_mot_de_passe), 256);
END$$

-- Droits par rôle
CREATE FUNCTION `fn_utilisateur_a_le_droit` (`p_id_utilisateur` INT UNSIGNED, `p_action` VARCHAR(20))
RETURNS TINYINT(1) READS SQL DATA
BEGIN
  DECLARE v_role   VARCHAR(30) DEFAULT NULL;
  DECLARE v_actif  BOOLEAN DEFAULT FALSE;
  DECLARE v_echecs INT DEFAULT 0;
  DECLARE CONTINUE HANDLER FOR NOT FOUND SET v_role = NULL;

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

-- Droit de l'utilisateur connecté (variable de session @id_utilisateur_courant)
CREATE FUNCTION `fn_a_le_droit` (`p_action` VARCHAR(20))
RETURNS TINYINT(1) READS SQL DATA
BEGIN
  RETURN fn_utilisateur_a_le_droit(@id_utilisateur_courant, p_action);
END$$

-- Exercice qui contient une date (NULL si aucun)
CREATE FUNCTION `fn_exercice_de_date` (`p_date` DATE)
RETURNS INT UNSIGNED READS SQL DATA
BEGIN
  RETURN (SELECT id_exercice FROM exercice
           WHERE p_date BETWEEN date_debut AND date_fin
           ORDER BY date_debut LIMIT 1);
END$$

-- Solde d'un compte à une date (NULL = toutes dates)
CREATE FUNCTION `fn_solde_compte` (`p_id_compte` INT UNSIGNED, `p_date` DATE)
RETURNS DECIMAL(15,2) READS SQL DATA
BEGIN
  DECLARE v_initial DECIMAL(15,2) DEFAULT 0;
  DECLARE v_mvt     DECIMAL(15,2) DEFAULT 0;
  DECLARE v_vir_in  DECIMAL(15,2) DEFAULT 0;
  DECLARE v_vir_out DECIMAL(15,2) DEFAULT 0;

  SET v_initial = (SELECT solde_initial FROM compte WHERE id_compte = p_id_compte);

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

-- Total déjà ventilé d'un mouvement
CREATE FUNCTION `fn_total_ventile` (`p_id_mouvement` INT UNSIGNED)
RETURNS DECIMAL(15,2) READS SQL DATA
BEGIN
  RETURN (SELECT COALESCE(SUM(montant), 0) FROM ventilation WHERE id_mouvement = p_id_mouvement);
END$$

-- Numéro de pièce suivant : PREFIXE-ANNEE-00001
CREATE PROCEDURE `sp_prochain_numero` (IN `p_prefixe` VARCHAR(5), IN `p_annee` SMALLINT UNSIGNED,
                                       OUT `p_numero` VARCHAR(20))
BEGIN
  DECLARE v_num INT UNSIGNED;

  INSERT INTO sequence_piece (prefixe, annee, dernier_numero)
  VALUES (p_prefixe, p_annee, 1)
  ON DUPLICATE KEY UPDATE dernier_numero = dernier_numero + 1;

  SELECT dernier_numero INTO v_num
    FROM sequence_piece WHERE prefixe = p_prefixe AND annee = p_annee;

  SET p_numero = CONCAT(p_prefixe, '-', p_annee, '-', LPAD(v_num, 5, '0'));
END$$

-- Bloque l'action si l'utilisateur connecté n'a pas le droit
CREATE PROCEDURE `sp_verifier_droit` (IN `p_action` VARCHAR(20))
BEGIN
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

-- Création d'un utilisateur.
-- Le tout premier utilisateur (table vide) doit être ADMIN et ne demande pas de connexion.
-- Ensuite, il faut être connecté avec le droit UTILISATEURS.
CREATE PROCEDURE `sp_utilisateur_creer` (IN `p_nom` VARCHAR(60), IN `p_prenoms` VARCHAR(100),
                                         IN `p_login` VARCHAR(50), IN `p_mot_de_passe` VARCHAR(255),
                                         IN `p_role` VARCHAR(30), IN `p_email` VARCHAR(150))
BEGIN
  DECLARE v_sel CHAR(32);

  IF (SELECT COUNT(*) FROM utilisateur) > 0 THEN
    CALL sp_verifier_droit('UTILISATEURS');
  ELSEIF p_role <> 'ADMIN' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Le premier utilisateur créé doit avoir le rôle ADMIN.';
  END IF;

  IF p_mot_de_passe IS NULL OR CHAR_LENGTH(p_mot_de_passe) < 8 THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Le mot de passe doit contenir au moins 8 caractères.';
  END IF;

  SET v_sel = LOWER(HEX(RANDOM_BYTES(16)));

  INSERT INTO utilisateur (nom, prenoms, login, mot_de_passe_hash, sel, role, email)
  VALUES (p_nom, p_prenoms, p_login, fn_hash_mot_de_passe(v_sel, p_mot_de_passe), v_sel, p_role, p_email);

  SELECT LAST_INSERT_ID() AS id_utilisateur;
END$$

-- Connexion : vérifie le mot de passe et renseigne @id_utilisateur_courant.
-- Le compte est bloqué après 5 échecs consécutifs.
CREATE PROCEDURE `sp_utilisateur_connecter` (IN `p_login` VARCHAR(50), IN `p_mot_de_passe` VARCHAR(255))
BEGIN
  DECLARE v_id     INT UNSIGNED DEFAULT NULL;
  DECLARE v_hash   CHAR(64);
  DECLARE v_sel    CHAR(32);
  DECLARE v_actif  TINYINT(1);
  DECLARE v_echecs TINYINT UNSIGNED;
  DECLARE CONTINUE HANDLER FOR NOT FOUND SET v_id = NULL;

  SET @id_utilisateur_courant = NULL;

  SELECT id_utilisateur, mot_de_passe_hash, sel, actif, tentatives_echouees
    INTO v_id, v_hash, v_sel, v_actif, v_echecs
    FROM utilisateur WHERE login = p_login;

  IF v_id IS NULL THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Identifiant ou mot de passe incorrect.';
  END IF;
  IF v_actif = 0 THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Ce compte est désactivé.';
  END IF;
  IF v_echecs >= 5 THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Compte bloqué après 5 tentatives échouées : contactez un administrateur.';
  END IF;

  IF fn_hash_mot_de_passe(v_sel, p_mot_de_passe) <> v_hash THEN
    UPDATE utilisateur SET tentatives_echouees = tentatives_echouees + 1
     WHERE id_utilisateur = v_id;
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Identifiant ou mot de passe incorrect.';
  END IF;

  UPDATE utilisateur
     SET tentatives_echouees = 0, derniere_connexion = NOW()
   WHERE id_utilisateur = v_id;

  SET @id_utilisateur_courant = v_id;

  SELECT id_utilisateur, nom, prenoms, login, role
    FROM utilisateur WHERE id_utilisateur = v_id;
END$$

-- Déconnexion
CREATE PROCEDURE `sp_utilisateur_deconnecter` ()
BEGIN
  SET @id_utilisateur_courant = NULL;
END$$

-- Déblocage d'un compte (remise à zéro des tentatives échouées) — réservé à l'ADMIN
CREATE PROCEDURE `sp_utilisateur_debloquer` (IN `p_id_utilisateur` INT UNSIGNED)
BEGIN
  CALL sp_verifier_droit('UTILISATEURS');
  UPDATE utilisateur SET tentatives_echouees = 0 WHERE id_utilisateur = p_id_utilisateur;
END$$

-- Contrôle interne : refuse toute opération sur un exercice clôturé
CREATE PROCEDURE `sp_controler_exercice` (IN `p_id_exercice` INT UNSIGNED)
BEGIN
  IF (SELECT statut FROM exercice WHERE id_exercice = p_id_exercice) = 'CLOTURE' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Exercice clôturé : aucune opération n''est possible sur cette période.';
  END IF;
END$$

-- Contrôle interne : refuse une sortie d'argent qui dépasse le solde + découvert autorisé
CREATE PROCEDURE `sp_controler_solde` (IN `p_id_compte` INT UNSIGNED, IN `p_montant` DECIMAL(15,2))
BEGIN
  DECLARE v_solde     DECIMAL(15,2);
  DECLARE v_decouvert DECIMAL(15,2);
  DECLARE v_msg       VARCHAR(128);

  SET v_solde     = fn_solde_compte(p_id_compte, NULL);
  SET v_decouvert = COALESCE((SELECT decouvert_autorise FROM compte WHERE id_compte = p_id_compte), 0);

  IF v_solde - p_montant < -v_decouvert THEN
    SET v_msg = CONCAT('Solde insuffisant : disponible ', v_solde + v_decouvert,
                       ', demandé ', p_montant, '.');
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = v_msg;
  END IF;
END$$

-- =====================================================================
-- TRIGGERS
-- =====================================================================

-- ---------------------------------------------------------------------
-- mouvement : avant insertion
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_mouvement_bi` BEFORE INSERT ON `mouvement` FOR EACH ROW
BEGIN
  DECLARE v_num VARCHAR(20);

  IF NEW.id_utilisateur_saisie IS NULL THEN
    SET NEW.id_utilisateur_saisie = @id_utilisateur_courant;
  END IF;

  IF NEW.id_exercice IS NULL THEN
    SET NEW.id_exercice = fn_exercice_de_date(NEW.date_mouvement);
  END IF;
  CALL sp_controler_exercice(NEW.id_exercice);

  IF (SELECT sens FROM categorie WHERE id_categorie = NEW.id_categorie) <> NEW.type_mouvement THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'La catégorie choisie ne correspond pas au type du mouvement (recette / dépense).';
  END IF;

  IF NEW.statut = 'ANNULE' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un mouvement ne peut pas être créé directement à l''état ANNULE.';
  END IF;

  IF NEW.statut = 'VALIDE' THEN
    IF NEW.type_mouvement = 'DEPENSE' THEN
      CALL sp_controler_solde(NEW.id_compte, NEW.montant);
    END IF;
    SET NEW.validated_at = NOW();
    SET NEW.id_utilisateur_validation = COALESCE(NEW.id_utilisateur_validation, @id_utilisateur_courant);
    IF NEW.numero_piece IS NULL THEN
      CALL sp_prochain_numero(IF(NEW.type_mouvement = 'RECETTE', 'REC', 'DEP'),
                              YEAR(NEW.date_mouvement), v_num);
      SET NEW.numero_piece = v_num;
    END IF;
  END IF;
END$$

-- ---------------------------------------------------------------------
-- mouvement : avant modification
--   BROUILLON -> modifiable librement, puis VALIDE ou ANNULE
--   VALIDE    -> seule l'annulation (avec motif) est permise
--   ANNULE    -> plus aucune modification
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_mouvement_bu` BEFORE UPDATE ON `mouvement` FOR EACH ROW
BEGIN
  DECLARE v_num VARCHAR(20);

  IF OLD.statut = 'ANNULE' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un mouvement annulé ne peut plus être modifié.';
  END IF;

  IF OLD.statut = 'VALIDE' AND (
       NEW.statut <> 'ANNULE'
    OR NEW.montant <> OLD.montant
    OR NEW.date_mouvement <> OLD.date_mouvement
    OR NEW.type_mouvement <> OLD.type_mouvement
    OR NEW.id_compte <> OLD.id_compte
    OR NEW.id_categorie <> OLD.id_categorie) THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Un mouvement validé ne peut pas être modifié : annulez-le puis ressaisissez-le.';
  END IF;

  IF NEW.date_mouvement <> OLD.date_mouvement THEN
    SET NEW.id_exercice = fn_exercice_de_date(NEW.date_mouvement);
  END IF;
  CALL sp_controler_exercice(OLD.id_exercice);
  CALL sp_controler_exercice(NEW.id_exercice);

  IF (SELECT sens FROM categorie WHERE id_categorie = NEW.id_categorie) <> NEW.type_mouvement THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'La catégorie choisie ne correspond pas au type du mouvement (recette / dépense).';
  END IF;

  -- Validation
  IF NEW.statut = 'VALIDE' AND OLD.statut <> 'VALIDE' THEN
    IF NEW.type_mouvement = 'DEPENSE' THEN
      CALL sp_controler_solde(NEW.id_compte, NEW.montant);
    END IF;
    SET NEW.validated_at = NOW();
    SET NEW.id_utilisateur_validation = COALESCE(@id_utilisateur_courant, NEW.id_utilisateur_validation);
    IF NEW.numero_piece IS NULL THEN
      CALL sp_prochain_numero(IF(NEW.type_mouvement = 'RECETTE', 'REC', 'DEP'),
                              YEAR(NEW.date_mouvement), v_num);
      SET NEW.numero_piece = v_num;
    END IF;
  END IF;

  -- Annulation
  IF NEW.statut = 'ANNULE' AND OLD.statut <> 'ANNULE' THEN
    IF NEW.motif_annulation IS NULL OR TRIM(NEW.motif_annulation) = '' THEN
      SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Le motif d''annulation est obligatoire.';
    END IF;
    SET NEW.annule_at = NOW();
    SET NEW.id_utilisateur_annulation = COALESCE(@id_utilisateur_courant, NEW.id_utilisateur_annulation);
  END IF;
END$$

-- ---------------------------------------------------------------------
-- mouvement : seuls les brouillons peuvent être supprimés
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_mouvement_bd` BEFORE DELETE ON `mouvement` FOR EACH ROW
BEGIN
  IF OLD.statut <> 'BROUILLON' THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Seuls les brouillons peuvent être supprimés : annulez plutôt ce mouvement.';
  END IF;
END$$

-- ---------------------------------------------------------------------
-- mouvement : journal d'audit
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_mouvement_ai` AFTER INSERT ON `mouvement` FOR EACH ROW
BEGIN
  INSERT INTO journal_audit (table_cible, id_enregistrement, action, details, id_utilisateur, utilisateur_sql)
  VALUES ('mouvement', NEW.id_mouvement, 'CREATION',
          JSON_OBJECT('type', NEW.type_mouvement, 'montant', NEW.montant, 'date', NEW.date_mouvement,
                      'compte', NEW.id_compte, 'categorie', NEW.id_categorie, 'statut', NEW.statut,
                      'numero_piece', NEW.numero_piece),
          (SELECT id_utilisateur FROM utilisateur WHERE id_utilisateur = @id_utilisateur_courant),
          USER());
END$$

CREATE TRIGGER `trg_mouvement_au` AFTER UPDATE ON `mouvement` FOR EACH ROW
BEGIN
  INSERT INTO journal_audit (table_cible, id_enregistrement, action, details, id_utilisateur, utilisateur_sql)
  VALUES ('mouvement', NEW.id_mouvement,
          CASE
            WHEN NEW.statut = 'VALIDE' AND OLD.statut <> 'VALIDE' THEN 'VALIDATION'
            WHEN NEW.statut = 'ANNULE' AND OLD.statut <> 'ANNULE' THEN 'ANNULATION'
            ELSE 'MODIFICATION'
          END,
          JSON_OBJECT('avant', JSON_OBJECT('montant', OLD.montant, 'date', OLD.date_mouvement,
                                           'compte', OLD.id_compte, 'categorie', OLD.id_categorie,
                                           'statut', OLD.statut, 'libelle', OLD.libelle),
                      'apres', JSON_OBJECT('montant', NEW.montant, 'date', NEW.date_mouvement,
                                           'compte', NEW.id_compte, 'categorie', NEW.id_categorie,
                                           'statut', NEW.statut, 'libelle', NEW.libelle,
                                           'numero_piece', NEW.numero_piece,
                                           'motif_annulation', NEW.motif_annulation)),
          (SELECT id_utilisateur FROM utilisateur WHERE id_utilisateur = @id_utilisateur_courant),
          USER());
END$$

CREATE TRIGGER `trg_mouvement_ad` AFTER DELETE ON `mouvement` FOR EACH ROW
BEGIN
  INSERT INTO journal_audit (table_cible, id_enregistrement, action, details, id_utilisateur, utilisateur_sql)
  VALUES ('mouvement', OLD.id_mouvement, 'SUPPRESSION',
          JSON_OBJECT('type', OLD.type_mouvement, 'montant', OLD.montant, 'date', OLD.date_mouvement,
                      'libelle', OLD.libelle),
          (SELECT id_utilisateur FROM utilisateur WHERE id_utilisateur = @id_utilisateur_courant),
          USER());
END$$

-- ---------------------------------------------------------------------
-- virement : avant insertion (exercice, solde, numéro VIR-AAAA-00001)
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_virement_bi` BEFORE INSERT ON `virement` FOR EACH ROW
BEGIN
  DECLARE v_num VARCHAR(20);

  IF NEW.id_utilisateur_saisie IS NULL THEN
    SET NEW.id_utilisateur_saisie = @id_utilisateur_courant;
  END IF;

  IF NEW.id_exercice IS NULL THEN
    SET NEW.id_exercice = fn_exercice_de_date(NEW.date_virement);
  END IF;
  CALL sp_controler_exercice(NEW.id_exercice);

  IF NEW.statut = 'ANNULE' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un virement ne peut pas être créé directement à l''état ANNULE.';
  END IF;

  CALL sp_controler_solde(NEW.id_compte_source, NEW.montant);

  IF NEW.numero_piece IS NULL THEN
    CALL sp_prochain_numero('VIR', YEAR(NEW.date_virement), v_num);
    SET NEW.numero_piece = v_num;
  END IF;
END$$

-- ---------------------------------------------------------------------
-- virement : seule l'annulation (avec motif) est permise
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_virement_bu` BEFORE UPDATE ON `virement` FOR EACH ROW
BEGIN
  IF OLD.statut = 'ANNULE' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un virement annulé ne peut plus être modifié.';
  END IF;

  IF NEW.statut <> 'ANNULE'
     OR NEW.montant <> OLD.montant
     OR NEW.date_virement <> OLD.date_virement
     OR NEW.id_compte_source <> OLD.id_compte_source
     OR NEW.id_compte_dest <> OLD.id_compte_dest THEN
    SIGNAL SQLSTATE '45000'
      SET MESSAGE_TEXT = 'Un virement ne peut pas être modifié : annulez-le puis ressaisissez-le.';
  END IF;

  CALL sp_controler_exercice(OLD.id_exercice);

  IF NEW.motif_annulation IS NULL OR TRIM(NEW.motif_annulation) = '' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Le motif d''annulation est obligatoire.';
  END IF;
END$$

CREATE TRIGGER `trg_virement_bd` BEFORE DELETE ON `virement` FOR EACH ROW
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un virement ne se supprime pas : annulez-le.';
END$$

CREATE TRIGGER `trg_virement_ai` AFTER INSERT ON `virement` FOR EACH ROW
BEGIN
  INSERT INTO journal_audit (table_cible, id_enregistrement, action, details, id_utilisateur, utilisateur_sql)
  VALUES ('virement', NEW.id_virement, 'CREATION',
          JSON_OBJECT('montant', NEW.montant, 'date', NEW.date_virement,
                      'source', NEW.id_compte_source, 'destination', NEW.id_compte_dest,
                      'numero_piece', NEW.numero_piece),
          (SELECT id_utilisateur FROM utilisateur WHERE id_utilisateur = @id_utilisateur_courant),
          USER());
END$$

CREATE TRIGGER `trg_virement_au` AFTER UPDATE ON `virement` FOR EACH ROW
BEGIN
  INSERT INTO journal_audit (table_cible, id_enregistrement, action, details, id_utilisateur, utilisateur_sql)
  VALUES ('virement', NEW.id_virement, 'ANNULATION',
          JSON_OBJECT('montant', NEW.montant, 'motif_annulation', NEW.motif_annulation),
          (SELECT id_utilisateur FROM utilisateur WHERE id_utilisateur = @id_utilisateur_courant),
          USER());
END$$

-- ---------------------------------------------------------------------
-- ventilation : le total ventilé ne peut pas dépasser le montant du mouvement
-- ---------------------------------------------------------------------
CREATE TRIGGER `trg_ventilation_bi` BEFORE INSERT ON `ventilation` FOR EACH ROW
BEGIN
  IF (SELECT statut FROM mouvement WHERE id_mouvement = NEW.id_mouvement) = 'ANNULE' THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Impossible de ventiler un mouvement annulé.';
  END IF;
  IF fn_total_ventile(NEW.id_mouvement) + NEW.montant
     > (SELECT montant FROM mouvement WHERE id_mouvement = NEW.id_mouvement) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Le total ventilé dépasserait le montant du mouvement.';
  END IF;
END$$

CREATE TRIGGER `trg_ventilation_bu` BEFORE UPDATE ON `ventilation` FOR EACH ROW
BEGIN
  IF fn_total_ventile(NEW.id_mouvement)
     - IF(OLD.id_mouvement = NEW.id_mouvement, OLD.montant, 0)
     + NEW.montant
     > (SELECT montant FROM mouvement WHERE id_mouvement = NEW.id_mouvement) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Le total ventilé dépasserait le montant du mouvement.';
  END IF;
END$$

DELIMITER ;

-- =====================================================================
-- VUE : solde actuel de chaque compte
-- =====================================================================
CREATE VIEW `v_solde_compte` AS
SELECT c.id_compte, c.code, c.libelle, c.type_compte, c.actif,
       c.solde_initial, c.decouvert_autorise,
       fn_solde_compte(c.id_compte, NULL) AS solde_actuel
  FROM compte c;

SET FOREIGN_KEY_CHECKS = 1;

-- =====================================================================
-- POUR DÉMARRER (à exécuter vous-même après l'import, avec vos valeurs) :
--
--   CALL sp_utilisateur_creer('NOM', 'Prénoms', 'admin', 'MotDePasseSolide', 'ADMIN', NULL);
--   CALL sp_utilisateur_connecter('admin', 'MotDePasseSolide');
--   INSERT INTO exercice (libelle, date_debut, date_fin) VALUES ('2026', '2026-01-01', '2026-12-31');
-- =====================================================================
