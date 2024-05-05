# EcoMove

Projet de formation CDA .NET chez Diginamic.

D�veloppement d'une application de location de v�hicules et de covoiturages.

## D�ploiement

Pour d�ployer le projet, ouvrez le Packager Manager Nuget et lancer les commandes suivantes :

```bash
  Add-Migration Initit
  Update-Database
```
Apr�s ces commandes la base de donn�es devrait �tre cr��e.

## Fixtures

Les tables suivantes ont �t� remplis avec des donn�es :
 - Brands
 - Models
 - Motorizations
 - Status
 - Categories
 - Vehicle


## R�le ADMIN

Voici les identifiants pour vous connecter avec un compte administrateur

`Login : admin@ecomove.com`

`Mot de passe : Azerty1!`

## R�le USER

Voici les identifiants pour vous connecter avec un compte utilisateur

`Login : user@ecomove.com` ou `user2@ecomove.com`

`Mot de passe : Azerty1!`

## Points restants � d�velopepr

- Envoi d'un email aux passagers d'un covoiturage si celui-ci est annul�
- Filtres de recherche
- Tests unitaires
- Refactoring
- Mot de passe plus fort
- L'Admin doit pouvoir modifier, supprimer une r�servation de v�hicule, un covoiturage...

