IF DB_ID(N'HospitalAppointmentDB') IS NOT NULL
BEGIN
    ALTER DATABASE HospitalAppointmentDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HospitalAppointmentDB;
END
GO

CREATE DATABASE HospitalAppointmentDB;
GO

USE HospitalAppointmentDB;
GO

CREATE TABLE Users (
    UserId INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Specialties (
    SpecialtyId INT IDENTITY PRIMARY KEY,
    SpecialtyName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);

CREATE TABLE Doctors (
    DoctorId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    SpecialtyId INT NOT NULL FOREIGN KEY REFERENCES Specialties(SpecialtyId),
    Qualification NVARCHAR(100),
    ExperienceYears INT NOT NULL DEFAULT 0,
    Description NVARCHAR(255)
);

CREATE TABLE Patients (
    PatientId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
    Gender NVARCHAR(10),
    DateOfBirth DATE,
    Address NVARCHAR(255),
    HealthInsuranceNo VARCHAR(50)
);

CREATE TABLE DoctorSchedules (
    ScheduleId INT IDENTITY PRIMARY KEY,
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(DoctorId),
    WorkDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    MaxPatients INT NOT NULL,
    CurrentPatients INT NOT NULL DEFAULT 0,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Available'
);

CREATE TABLE MedicalServices (
    MedicalServiceId INT IDENTITY PRIMARY KEY,
    ServiceName NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL DEFAULT 0,
    Description NVARCHAR(255)
);

CREATE TABLE ClinicRooms (
    ClinicRoomId INT IDENTITY PRIMARY KEY,
    SpecialtyId INT NOT NULL FOREIGN KEY REFERENCES Specialties(SpecialtyId),
    RoomName NVARCHAR(100) NOT NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Active'
);

CREATE TABLE Appointments (
    AppointmentId INT IDENTITY PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES Patients(PatientId),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES Doctors(DoctorId),
    ScheduleId INT NOT NULL FOREIGN KEY REFERENCES DoctorSchedules(ScheduleId),
    MedicalServiceId INT NOT NULL FOREIGN KEY REFERENCES MedicalServices(MedicalServiceId),
    ClinicRoomId INT NOT NULL FOREIGN KEY REFERENCES ClinicRooms(ClinicRoomId),
    Reason NVARCHAR(255),
    Status NVARCHAR(30) NOT NULL DEFAULT N'Pending',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE AppointmentLogs (
    LogId INT IDENTITY PRIMARY KEY,
    AppointmentId INT NOT NULL,
    ActionName NVARCHAR(50) NOT NULL,
    ActionDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO
