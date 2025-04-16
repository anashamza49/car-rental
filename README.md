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


---

## 🔐 Authentification (JWT)
- `POST /api/auth/register` : enregistrement
- `POST /api/auth/login` : connexion

## 🚗 Gestion des voitures
- `GET /api/cars` : liste des voitures (Admin ou Employee)
- `GET /api/cars/{id}` : détails d’une voiture
- `POST /api/cars` : ajout (Admin uniquement)
- `PUT /api/cars/{id}` : modification (Admin uniquement)
- `DELETE /api/cars/{id}` : suppression (Admin uniquement)

## 📦 Location
- `POST /api/Rental/rent` : louer une voiture
- `GET /api/Rental` : liste des locations


---

![Capture](https://github.com/user-attachments/assets/9a15e311-4028-44a9-8eb5-6524a3785d87)

![Capture2](https://github.com/user-attachments/assets/c893ab52-d87c-4349-908e-923da8075b61)

![Capture3](https://github.com/user-attachments/assets/736ed07d-a0f3-4e68-9588-b6605dfb75eb)

![Capture4](https://github.com/user-attachments/assets/3dffe150-1a4d-453f-b690-7f628b25a6cb)

![Capture](https://github.com/user-attachments/assets/e5b349d5-ef5b-4d77-b4eb-e203236c00f0)

![Capture](https://github.com/user-attachments/assets/9d5b1279-665f-464b-947f-1314b02eca13)



## 🛠️ Stack technique
- ASP.NET Core Web API
- JWT Authentication
- C#
- Entity Framework Core

