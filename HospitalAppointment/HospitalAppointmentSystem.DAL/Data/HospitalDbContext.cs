using HospitalAppointmentSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.DAL.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentLog> AppointmentLogs => Set<AppointmentLog>();
    public DbSet<MedicalService> MedicalServices => Set<MedicalService>();
    public DbSet<ClinicRoom> ClinicRooms => Set<ClinicRoom>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Specialty>().ToTable("Specialties");
        modelBuilder.Entity<Doctor>().ToTable("Doctors");
        modelBuilder.Entity<Patient>().ToTable("Patients");
        modelBuilder.Entity<DoctorSchedule>().ToTable("DoctorSchedules");
        modelBuilder.Entity<Appointment>().ToTable("Appointments");
        modelBuilder.Entity<AppointmentLog>().ToTable("AppointmentLogs");
        modelBuilder.Entity<MedicalService>().ToTable("MedicalServices");
        modelBuilder.Entity<ClinicRoom>().ToTable("ClinicRooms");

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.UserId);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithOne(u => u.Patient)
            .HasForeignKey<Patient>(p => p.UserId);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Specialty)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecialtyId);

        modelBuilder.Entity<DoctorSchedule>()
            .HasOne(s => s.Doctor)
            .WithMany(d => d.DoctorSchedules)
            .HasForeignKey(s => s.DoctorId);

        // Khai báo rõ các khóa ngoại của Appointment để EF không tự tạo cột ảo
        // như DoctorScheduleScheduleId. Bảng SQL Server đang dùng cột ScheduleId.
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.DoctorSchedule)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ScheduleId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.MedicalService)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.MedicalServiceId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.ClinicRoom)
            .WithMany(r => r.Appointments)
            .HasForeignKey(a => a.ClinicRoomId);

        modelBuilder.Entity<ClinicRoom>()
            .HasOne(r => r.Specialty)
            .WithMany()
            .HasForeignKey(r => r.SpecialtyId);
    }
}
