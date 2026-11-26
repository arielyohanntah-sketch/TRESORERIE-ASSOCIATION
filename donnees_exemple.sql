-- =====================================================================
--  Trésorerie Asso — DONNÉES D'EXEMPLE (exercice 2026)
--  À importer UNE SEULE FOIS dans phpMyAdmin, base « tresorerie_asso »,
--  après tresorerie_asso_v2.sql. Vos utilisateurs existants sont conservés.
--
--  Ajoute : 4 utilisateurs de démonstration, l'exercice 2026, 3 comptes,
--  12 catégories, 9 tiers, 5 centres analytiques et leurs budgets,
--  30 opérations validées (avec ventilation et pièces), 1 opération annulée,
--  4 virements et 3 brouillons à valider.
--
--  Comptes de démonstration (mot de passe pour tous : Demo2026!)
--    president  -> Président          tresorier -> Trésorier
--    adjoint    -> Trésorier adjoint  membre    -> Membre du bureau
-- =====================================================================

SET NAMES utf8mb4;
USE `tresorerie_asso`;

-- ---------------------------------------------------------------------
-- Utilisateurs de démonstration (ignorés si l'identifiant existe déjà)
-- ---------------------------------------------------------------------
INSERT IGNORE INTO utilisateur (nom, prenoms, login, mot_de_passe_hash, sel, role) VALUES
 ('KOFFI',   'Jean-Marc', 'president', fn_hash_mot_de_passe('5f2c8a1e9b7d4c3a6e0f1b2d3c4a5e6f', 'Demo2026!'), '5f2c8a1e9b7d4c3a6e0f1b2d3c4a5e6f', 'PRESIDENT'),
 ('KONAN',   'Ama',       'tresorier', fn_hash_mot_de_passe('a3b9c1d7e5f2a8b4c6d0e9f1a2b3c4d5', 'Demo2026!'), 'a3b9c1d7e5f2a8b4c6d0e9f1a2b3c4d5', 'TRESORIER'),
 ('KOUASSI', 'Yao',       'adjoint',   fn_hash_mot_de_passe('0e1d2c3b4a5f6e7d8c9b0a1f2e3d4c5b', 'Demo2026!'), '0e1d2c3b4a5f6e7d8c9b0a1f2e3d4c5b', 'TRESORIER_ADJOINT'),
 ('BAMBA',   'Fanta',     'membre',    fn_hash_mot_de_passe('9a8b7c6d5e4f3a2b1c0d9e8f7a6b5c4d', 'Demo2026!'), '9a8b7c6d5e4f3a2b1c0d9e8f7a6b5c4d', 'MEMBRE_BUREAU');

SET @pres = (SELECT id_utilisateur FROM utilisateur WHERE login = 'president');
SET @tres = (SELECT id_utilisateur FROM utilisateur WHERE login = 'tresorier');
SET @adj  = (SELECT id_utilisateur FROM utilisateur WHERE login = 'adjoint');

-- ---------------------------------------------------------------------
-- Exercice 2026 (seulement s'il n'en existe pas déjà un pour 2026)
-- ---------------------------------------------------------------------
INSERT INTO exercice (libelle, date_debut, date_fin)
SELECT '2026', '2026-01-01', '2026-12-31' FROM DUAL
 WHERE NOT EXISTS (SELECT 1 FROM exercice WHERE '2026-06-01' BETWEEN date_debut AND date_fin);
SET @ex = (SELECT id_exercice FROM exercice WHERE '2026-06-01' BETWEEN date_debut AND date_fin ORDER BY date_debut LIMIT 1);

-- ---------------------------------------------------------------------
-- Comptes
-- ---------------------------------------------------------------------
INSERT IGNORE INTO compte (code, libelle, type_compte, etablissement, numero_compte, solde_initial, decouvert_autorise, date_ouverture) VALUES
 ('CAI', 'Caisse principale', 'CAISSE',       NULL,                 NULL,               150000,      0, '2026-01-01'),
 ('BQ',  'Compte bancaire',   'BANQUE',       'Banque partenaire',  'CI000 00000 0001', 800000, 200000, '2026-01-01'),
 ('MM',  'Mobile Money',      'MOBILE_MONEY', 'Opérateur mobile',   '07 00 00 00 00',        0,      0, '2026-01-01');

SET @cai = (SELECT id_compte FROM compte WHERE code = 'CAI');
SET @bq  = (SELECT id_compte FROM compte WHERE code = 'BQ');
SET @mm  = (SELECT id_compte FROM compte WHERE code = 'MM');

-- ---------------------------------------------------------------------
-- Catégories
-- ---------------------------------------------------------------------
INSERT IGNORE INTO categorie (code, libelle, sens) VALUES
 ('COT', 'Cotisations des membres', 'RECETTE'),
 ('DON', 'Dons',                    'RECETTE'),
 ('SUB', 'Subventions',             'RECETTE'),
 ('VEN', 'Ventes',                  'RECETTE'),
 ('ACT', 'Recettes d''activités',   'RECETTE'),
 ('FOU', 'Fournitures et matériel', 'DEPENSE'),
 ('TRA', 'Transport et missions',   'DEPENSE'),
 ('LOC', 'Locations',               'DEPENSE'),
 ('COM', 'Communication',           'DEPENSE'),
 ('SOC', 'Actions sociales',        'DEPENSE'),
 ('CHG', 'Charges (eau, électricité)', 'DEPENSE'),
 ('FBQ', 'Frais bancaires',         'DEPENSE');

SET @cot = (SELECT id_categorie FROM categorie WHERE code = 'COT');
SET @don = (SELECT id_categorie FROM categorie WHERE code = 'DON');
SET @sub = (SELECT id_categorie FROM categorie WHERE code = 'SUB');
SET @ven = (SELECT id_categorie FROM categorie WHERE code = 'VEN');
SET @act = (SELECT id_categorie FROM categorie WHERE code = 'ACT');
SET @fou = (SELECT id_categorie FROM categorie WHERE code = 'FOU');
SET @tra = (SELECT id_categorie FROM categorie WHERE code = 'TRA');
SET @loc = (SELECT id_categorie FROM categorie WHERE code = 'LOC');
SET @com = (SELECT id_categorie FROM categorie WHERE code = 'COM');
SET @soc = (SELECT id_categorie FROM categorie WHERE code = 'SOC');
SET @chg = (SELECT id_categorie FROM categorie WHERE code = 'CHG');
SET @fbq = (SELECT id_categorie FROM categorie WHERE code = 'FBQ');

-- ---------------------------------------------------------------------
-- Tiers (ajoutés seulement s'ils n'existent pas)
-- ---------------------------------------------------------------------
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Mairie de la commune', 'PARTENAIRE', '27 20 00 00 01' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Mairie de la commune');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Librairie du Centre', 'FOURNISSEUR', '27 20 00 00 02' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Librairie du Centre');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Imprimerie Moderne', 'FOURNISSEUR', '27 20 00 00 03' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Imprimerie Moderne');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Centre culturel municipal', 'FOURNISSEUR', '27 20 00 00 04' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Centre culturel municipal');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Compagnie d''électricité', 'FOURNISSEUR', NULL FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Compagnie d''électricité');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Station-service Avenir', 'FOURNISSEUR', NULL FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Station-service Avenir');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'M. Kouamé Yao', 'DONATEUR', '07 00 00 00 07' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'M. Kouamé Yao');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Entreprise partenaire SARL', 'PARTENAIRE', NULL FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Entreprise partenaire SARL');
INSERT INTO tiers (nom, type_tiers, telephone) SELECT 'Membres de la section A', 'MEMBRE', NULL FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tiers WHERE nom = 'Membres de la section A');

SET @t_mairie = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Mairie de la commune');
SET @t_libr   = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Librairie du Centre');
SET @t_impr   = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Imprimerie Moderne');
SET @t_centre = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Centre culturel municipal');
SET @t_elec   = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Compagnie d''électricité');
SET @t_stat   = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Station-service Avenir');
SET @t_kouame = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'M. Kouamé Yao');
SET @t_entr   = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Entreprise partenaire SARL');
SET @t_secA   = (SELECT MIN(id_tiers) FROM tiers WHERE nom = 'Membres de la section A');

-- ---------------------------------------------------------------------
-- Centres analytiques et budgets 2026
-- ---------------------------------------------------------------------
INSERT IGNORE INTO centre_analytique (code, libelle, description, responsable) VALUES
 ('FON', 'Fonctionnement',   'Frais généraux du siège',               'Ama KONAN'),
 ('EVT', 'Événements',       'Assemblées, gala, tournois',            'Jean-Marc KOFFI'),
 ('SOC', 'Actions sociales', 'Kits scolaires, aides aux familles',    'Yao KOUASSI'),
 ('COM', 'Communication',    'Affiches, radio, site internet',        'Fanta BAMBA'),
 ('MIS', 'Missions terrain', 'Déplacements et missions',              'Yao KOUASSI');

SET @fon = (SELECT id_centre FROM centre_analytique WHERE code = 'FON');
SET @evt = (SELECT id_centre FROM centre_analytique WHERE code = 'EVT');
SET @csoc = (SELECT id_centre FROM centre_analytique WHERE code = 'SOC');
SET @ccom = (SELECT id_centre FROM centre_analytique WHERE code = 'COM');
SET @mis = (SELECT id_centre FROM centre_analytique WHERE code = 'MIS');

INSERT IGNORE INTO budget_centre (id_centre, id_exercice, recettes_prevues, depenses_prevues) VALUES
 (@fon,  @ex,       0,  400000),
 (@evt,  @ex, 1000000,  300000),
 (@csoc, @ex, 1000000,  800000),
 (@ccom, @ex,       0,  150000),
 (@mis,  @ex,       0,  200000);

-- =====================================================================
-- OPÉRATIONS VALIDÉES (saisies et validées par la trésorière)
-- Les numéros de pièce (REC-2026-…, DEP-2026-…) sont attribués par la base.
-- =====================================================================
SET @id_utilisateur_courant = @tres;

-- Janvier
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-01-10', @cai, @cot, @t_secA, 120000, 'ESPECES', 'RC-0101', 'Cotisations de janvier', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 120000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'RC-0101', '2026-01-10', 120000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-01-15', @cai, @fou, @t_libr, 35000, 'ESPECES', 'FA-2026-014', 'Fournitures de bureau', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 35000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'FA-2026-014', '2026-01-15', 35000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-01-20', @bq, @sub, @t_mairie, 1000000, 'VIREMENT', 'VIR-MAIRIE-01', 'Subvention de la mairie (1re tranche)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @csoc, 600000), (@m, @evt, 400000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'CONTRAT', 'CONV-2026-01', '2026-01-05', 1500000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-01-31', @bq, @fbq, NULL, 6500, 'VIREMENT', NULL, 'Frais bancaires de janvier', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 6500);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RELEVE', 'RLV-2026-01', '2026-01-31', 6500);

INSERT INTO virement (date_virement, id_compte_source, id_compte_dest, montant, libelle, id_utilisateur_saisie)
VALUES ('2026-02-05', @cai, @bq, 150000, 'Dépôt des espèces à la banque', @tres);

-- Février
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-02-12', @cai, @cot, @t_secA, 95000, 'ESPECES', 'RC-0201', 'Cotisations de février', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 95000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-02-18', @bq, @loc, @t_centre, 150000, 'CHEQUE', 'CHQ 0001245', 'Location de la salle pour l''assemblée générale', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @evt, 150000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'CCM-2026-031', '2026-02-16', 150000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-02-25', @cai, @tra, NULL, 40000, 'ESPECES', NULL, 'Transport de la délégation (réunion régionale)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @mis, 40000);

-- Mars
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-03-08', @mm, @don, @t_kouame, 250000, 'MOBILE_MONEY', 'MM260308.1452', 'Don reçu lors de la soirée de gala', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @evt, 250000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'RD-2026-003', '2026-03-08', 250000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-03-15', @bq, @com, @t_impr, 85000, 'VIREMENT', 'IM-2026-112', 'Impression d''affiches et de banderoles', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @ccom, 85000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'IM-2026-112', '2026-03-14', 85000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-03-22', @mm, @soc, @t_libr, 120000, 'MOBILE_MONEY', 'MM260322.0931', 'Kits scolaires (1er lot)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @csoc, 120000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'FA-2026-077', '2026-03-21', 120000);

-- Avril
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-04-05', @cai, @cot, @t_secA, 110000, 'ESPECES', 'RC-0401', 'Cotisations d''avril', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 110000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'RC-0401', '2026-04-05', 110000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-04-18', @bq, @chg, @t_elec, 42000, 'VIREMENT', 'ELEC-0426', 'Facture d''électricité du siège (mars-avril)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 42000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'ELEC-0426', '2026-04-10', 42000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-04-26', @cai, @ven, NULL, 135000, 'ESPECES', NULL, 'Vente de tee-shirts de l''association', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @evt, 135000);

-- Mai
INSERT INTO virement (date_virement, id_compte_source, id_compte_dest, montant, libelle, id_utilisateur_saisie)
VALUES ('2026-05-03', @cai, @bq, 250000, 'Dépôt des espèces à la banque', @tres);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-05-14', @bq, @soc, NULL, 300000, 'VIREMENT', NULL, 'Aide aux familles sinistrées', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @csoc, 300000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'AUTRE', 'PV-BUREAU-0512', '2026-05-12', 300000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-05-28', @cai, @tra, @t_stat, 55000, 'ESPECES', NULL, 'Carburant pour la mission terrain', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @mis, 55000);

-- Juin (validées par le président)
SET @id_utilisateur_courant = @pres;
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-06-10', @bq, @sub, @t_mairie, 500000, 'VIREMENT', 'VIR-MAIRIE-02', 'Subvention de la mairie (2e tranche)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @csoc, 300000), (@m, @evt, 200000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'CONTRAT', 'CONV-2026-01', '2026-01-05', 1500000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-06-20', @mm, @act, NULL, 180000, 'MOBILE_MONEY', NULL, 'Recettes du tournoi de football', 'VALIDE', @adj);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @evt, 180000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-06-27', @mm, @loc, NULL, 75000, 'MOBILE_MONEY', 'MM260627.1810', 'Location du matériel de sonorisation (tournoi)', 'VALIDE', @adj);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @evt, 75000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'SONO-0627', '2026-06-27', 75000);
SET @id_utilisateur_courant = @tres;

-- Juillet
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-07-08', @cai, @cot, @t_secA, 90000, 'ESPECES', 'RC-0701', 'Cotisations de juillet', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 90000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-07-19', @bq, @fou, @t_libr, 64000, 'CHEQUE', 'CHQ 0001246', 'Achat d''une imprimante', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 64000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'FA-2026-201', '2026-07-18', 64000);

-- Août
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-08-12', @bq, @soc, @t_libr, 210000, 'VIREMENT', 'FA-2026-233', 'Kits scolaires (2e lot)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @csoc, 210000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'FA-2026-233', '2026-08-10', 210000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-08-30', @bq, @fbq, NULL, 7200, 'VIREMENT', NULL, 'Frais bancaires du semestre', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 7200);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RELEVE', 'RLV-2026-08', '2026-08-31', 7200);

-- Septembre
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-09-06', @cai, @cot, @t_secA, 130000, 'ESPECES', 'RC-0901', 'Cotisations de la rentrée', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 130000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'RC-0901', '2026-09-06', 130000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-09-21', @cai, @com, NULL, 30000, 'ESPECES', NULL, 'Annonce sur la radio locale', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @ccom, 30000);

INSERT INTO virement (date_virement, id_compte_source, id_compte_dest, montant, libelle, id_utilisateur_saisie)
VALUES ('2026-09-25', @bq, @mm, 100000, 'Alimentation du compte Mobile Money', @tres);

-- Octobre
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-10-10', @bq, @don, @t_entr, 400000, 'VIREMENT', 'VIR-ENT-1010', 'Don d''une entreprise partenaire', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @csoc, 200000), (@m, @fon, 200000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'RD-2026-010', '2026-10-10', 400000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-10-24', @mm, @tra, NULL, 48000, 'MOBILE_MONEY', 'MM261024.0745', 'Déplacement pour une formation', 'VALIDE', @adj);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @mis, 48000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'TR-1024', '2026-10-24', 48000);

-- Une facture saisie avec un mauvais montant, puis annulée par le président
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-10-30', @bq, @chg, @t_elec, 41000, 'VIREMENT', 'ELEC-1026', 'Facture d''électricité du siège (erreur de montant)', 'VALIDE', @tres);
SET @m_annule = LAST_INSERT_ID();
SET @id_utilisateur_courant = @pres;
UPDATE mouvement SET statut = 'ANNULE', motif_annulation = 'Montant erroné : la facture fait 40 100 FCFA'
 WHERE id_mouvement = @m_annule;
SET @id_utilisateur_courant = @tres;

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-10-30', @bq, @chg, @t_elec, 40100, 'VIREMENT', 'ELEC-1026', 'Facture d''électricité du siège (sept.-oct.)', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 40100);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'ELEC-1026', '2026-10-25', 40100);

-- Novembre
INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-11-05', @cai, @cot, @t_secA, 85000, 'ESPECES', 'RC-1101', 'Cotisations de novembre', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 85000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'RECU', 'RC-1101', '2026-11-05', 85000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-11-12', @cai, @fou, @t_libr, 32500, 'ESPECES', 'FA-2026-301', 'Fournitures de bureau', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 32500);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'FA-2026-301', '2026-11-12', 32500);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-11-18', @bq, @com, NULL, 68000, 'CARTE', NULL, 'Hébergement annuel du site internet', 'VALIDE', @tres);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @ccom, 68000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'WEB-2026-11', '2026-11-18', 68000);

INSERT INTO virement (date_virement, id_compte_source, id_compte_dest, montant, libelle, id_utilisateur_saisie)
VALUES ('2026-11-20', @cai, @bq, 200000, 'Dépôt des espèces à la banque', @tres);

-- =====================================================================
-- BROUILLONS saisis par le trésorier adjoint (en attente de validation)
-- =====================================================================
SET @id_utilisateur_courant = @adj;

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-11-24', @bq, @loc, @t_centre, 150000, 'CHEQUE', NULL, 'Location de la salle pour l''AG de fin d''année', 'BROUILLON', @adj);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @evt, 150000);
INSERT INTO piece_justificative (id_mouvement, type_piece, reference, date_piece, montant) VALUES (@m, 'FACTURE', 'CCM-2026-118', '2026-11-22', 150000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('DEPENSE', '2026-11-25', @cai, @tra, @t_stat, 45000, 'ESPECES', 'Reçu 004512', 'Carburant pour la mission de Bouaké', 'BROUILLON', @adj);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @mis, 45000);

INSERT INTO mouvement (type_mouvement, date_mouvement, id_compte, id_categorie, id_tiers, montant, mode_paiement, reference_externe, libelle, statut, id_utilisateur_saisie)
VALUES ('RECETTE', '2026-11-25', @cai, @cot, @t_secA, 60000, 'ESPECES', 'RC-1102', 'Cotisations des retardataires', 'BROUILLON', @adj);
SET @m = LAST_INSERT_ID();
INSERT INTO ventilation (id_mouvement, id_centre, montant) VALUES (@m, @fon, 60000);

SET @id_utilisateur_courant = NULL;

-- Résultat attendu après import (soldes) :
--   Caisse principale 122 500 · Compte bancaire 2 227 200 · Mobile Money 287 000
SELECT code, libelle, fn_solde_compte(id_compte, NULL) AS solde_actuel FROM compte WHERE code IN ('CAI', 'BQ', 'MM');
