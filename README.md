# Employees Management

Une application web de gestion des employés avec authentification sécurisée via JWT.

## 📝 Description

Ce projet permet la gestion des employés dans une entreprise, avec une séparation des rôles (Admin / Employé), une interface de gestion des voitures associées, et une API sécurisée.

---

## ✅ Fonctionnalités confirmées

### 🔐 Authentification
- `POST /api/auth/register` : inscription avec rôle (`Admin` ou `Employee`)
- `POST /api/auth/login` : connexion et génération de token JWT
- Politiques d'accès :
  - `AdminOnly`
  - `EmployeeOrAdmin`

### 👥 Gestion des employés (prévu ou en cours)
- Création, lecture, mise à jour, suppression (CRUD)
- Attribution de rôles/postes

### 🏢 Gestion des départements/services (prévu)

### 🔍 Recherche et filtrage (prévu)

### 📊 Dashboard RH (facultatif)

---

## 🚗 Gestion des voitures
- `GET /api/cars` : liste des voitures de l’utilisateur (Admin ou Employee)
- `POST /api/cars` : ajout d’une voiture (Admin uniquement)
- `DELETE /api/cars/{id}` : suppression d’une voiture (Admin uniquement)

---
![Capture1](https://github.com/user-attachments/assets/b9381813-283c-4c69-8b73-df52193f9dab)

![Capture2](https://github.com/user-attachments/assets/c893ab52-d87c-4349-908e-923da8075b61)

![Capture3](https://github.com/user-attachments/assets/736ed07d-a0f3-4e68-9588-b6605dfb75eb)

![Capture4](https://github.com/user-attachments/assets/3dffe150-1a4d-453f-b690-7f628b25a6cb)

![Capture5](https://github.com/user-attachments/assets/c45c154e-7795-4c6e-b4e2-fa0c54bf023e)

![Capture6](https://github.com/user-attachments/assets/d5a2acfb-e003-460a-b886-2b6ad9eb13c0)



## 🛠️ Stack technique
- ASP.NET Core Web API
- JWT Authentication
- C#
- Entity Framework Core


## 📌 Auteur

Hamzaoui – 2025
