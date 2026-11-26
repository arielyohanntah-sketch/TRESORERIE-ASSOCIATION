# Trésorerie Asso

Projet L2 — Base de données : gestion de la trésorerie d'une association
(base MySQL + application Windows Forms en C# / .NET 10).

## Contenu du dépôt

| Élément | Rôle |
|---|---|
| `tresorerie_asso_v2.sql` | Script complet de la base (tables, triggers, procédures, vue) — à importer dans phpMyAdmin |
| `donnees_exemple.sql` | Données de démonstration (comptes `president`, `tresorier`, `adjoint`, `membre`, mot de passe `Demo2026!`) |
| `vider_donnees.sql` | Vide toutes les tables (garde la structure) |
| `tresorerie_asso.sql` | Ancienne version du script (historique) |
| `SLN_TRESORERIE_ASSO/` | Application Windows Forms (ouvrir `SLN_TRESORERIE_ASSO.slnx`) |

## Installation rapide

1. Démarrez WampServer, puis importez `tresorerie_asso_v2.sql` dans phpMyAdmin
   (et `donnees_exemple.sql` si vous voulez des données de test).
2. Dans `SLN_TRESORERIE_ASSO/Data/`, copiez `Connexion.local.exemple.txt` en
   **`Connexion.local.cs`** et mettez-y votre mot de passe MySQL
   (ce fichier n'est pas envoyé sur GitHub).
3. Ouvrez `SLN_TRESORERIE_ASSO/SLN_TRESORERIE_ASSO.slnx` dans Visual Studio et lancez (F5).

Plus de détails (rôles, organisation du code) : [SLN_TRESORERIE_ASSO/LISEZMOI.md](SLN_TRESORERIE_ASSO/LISEZMOI.md).
