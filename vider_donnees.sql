-- =====================================================================
--  Trésorerie Asso — VIDER TOUTES LES DONNÉES (garde la structure)
--
--  ⚠ Efface définitivement : opérations, virements, ventilations, pièces,
--    journal d'audit, comptes, catégories, tiers, centres, budgets, exercices,
--    numérotation des pièces, et les 4 comptes de démonstration.
--  ✔ Conserve : les tables, procédures, triggers, et VOS utilisateurs
--    (pour pouvoir vous reconnecter).
--
--  À importer dans phpMyAdmin, base « tresorerie_asso ».
-- =====================================================================

USE `tresorerie_asso`;

-- TRUNCATE vide une table sans déclencher les triggers (qui interdisent
-- normalement de supprimer une opération validée) et remet les compteurs à 1.
SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE ventilation;
TRUNCATE TABLE piece_justificative;
TRUNCATE TABLE mouvement;
TRUNCATE TABLE virement;
TRUNCATE TABLE journal_audit;
TRUNCATE TABLE sequence_piece;
TRUNCATE TABLE budget_centre;
TRUNCATE TABLE centre_analytique;
TRUNCATE TABLE categorie;
TRUNCATE TABLE tiers;
TRUNCATE TABLE compte;
TRUNCATE TABLE exercice;

-- Comptes de démonstration ajoutés par donnees_exemple.sql
DELETE FROM utilisateur WHERE login IN ('president', 'tresorier', 'adjoint', 'membre');

SET FOREIGN_KEY_CHECKS = 1;

SELECT 'Données effacées. Il reste vos utilisateurs :' AS info;
SELECT nom, prenoms, login, role FROM utilisateur;
