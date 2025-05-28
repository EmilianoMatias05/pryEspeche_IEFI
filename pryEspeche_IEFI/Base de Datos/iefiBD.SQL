---Primero esto

CREATE DATABASE iefiBD;

---Despues esto:

USE iefiBD;

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Contraseña VARCHAR(255) NOT NULL
);

-- Crear tabla Auditoria SIN AUTO_INCREMENT
CREATE TABLE Auditoria (
    Id INT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Fecha DATE NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    Tiempo TIME NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- Insertar registro en Usuarios con Id = 1
INSERT INTO Usuarios (Id, Nombre, Contraseña)
VALUES (1, 'emi', '2222');