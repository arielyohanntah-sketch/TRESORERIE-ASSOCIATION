# Trésorerie Asso — application Windows Forms

## Démarrer

1. **Base de données** : dans phpMyAdmin, importez `..\tresorerie_asso_v2.sql`
   (⚠ il recrée toutes les tables de `tresorerie_asso`, les données existantes sont effacées).
2. Ouvrez `SLN_TRESORERIE_ASSO.slnx` dans Visual Studio.
3. Dans `Data/`, copiez `Connexion.local.exemple.txt` sous le nom **`Connexion.local.cs`**
   et mettez-y le mot de passe MySQL (le même que dans phpMyAdmin), puis lancez (F5).
   Ce fichier est exclu de Git : le mot de passe n'est jamais publié sur GitHub.
   Serveur, port, base et utilisateur restent dans `Data/Connexion.cs`.
4. Base vide : sur l'écran de connexion, cliquez **Créer le compte administrateur**.
5. Ensuite, dans **Paramétrage** : créez l'exercice, les comptes (caisse, banque…),
   les catégories, les centres analytiques et les budgets. Vous pouvez alors saisir
   les recettes, dépenses et virements.

## Rôles et espaces

| Rôle | Saisir | Valider / annuler | Virements | Paramétrage | Clôturer | Utilisateurs | Journal d'audit |
|---|:-:|:-:|:-:|:-:|:-:|:-:|:-:|
| Administrateur | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ |
| Président | | ✔ | ✔ | ✔ | ✔ | | ✔ |
| Trésorier | ✔ | ✔ | ✔ | ✔ | | | |
| Trésorier adjoint | ✔ | | ✔ | | | | |
| Commissaire aux comptes | | | | | | | ✔ |
| Membre du bureau | | | | | | | |

Tout le monde consulte le tableau de bord, les opérations validées, les virements, les soldes et le budget.
Chaque utilisateur a **Mon espace** : *Mes brouillons*, *Mes saisies*, *À valider* (valideurs) et *Mon activité*.
Un brouillon n'est visible que par son auteur et par les valideurs, et seul son auteur peut le modifier ou le supprimer.
Le menu et les boutons n'affichent que ce que le rôle a le droit de faire.

## Organisation du code

| Dossier | Contenu |
|---|---|
| `Data/` | Connexion MySQL (`Db`), utilisateur connecté (`Session`), toutes les requêtes (`Depot`), paramètres de connexion (`Connexion`) |
| `UI/` | Couleurs, polices, icônes et styles des grilles (`Theme`), messages et export CSV (`Ui`) |
| `Forms/` | Fenêtres (chargement, connexion, principale, saisie, virement…) : mise en page dans les fichiers `.Designer.cs`, modifiable dans le concepteur de Visual Studio |
| `Views/` | Écrans affichés au centre de la fenêtre principale, eux aussi modifiables dans le concepteur |

Les règles de gestion (soldes, exercice clôturé, numérotation des pièces, droits, audit)
sont appliquées par la base (triggers et procédures du script v2) : l'application affiche
simplement leurs messages d'erreur.
