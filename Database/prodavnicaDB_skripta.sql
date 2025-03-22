create database ProdavnicaDB;
go

use ProdavnicaDB;
go

create table Osoba (
    osoba_id int primary key identity(1,1),
    ime nvarchar(100),
    prezime nvarchar(100),
    jmbg nvarchar(13) unique,
    telefon nvarchar(20),
	username nvarchar(40),
	sifra nvarchar(40),
	uloga nvarchar(30)
);
create table Kategorija (
    kategorija_id int primary key identity(1,1),
    naziv nvarchar(100)
);

create table Proizvod (
    proizvod_id int primary key identity(1,1),
    naziv nvarchar(100),
    cena decimal(10, 2),
	kolicina int,
	barkod nvarchar(50),
	kategorija_id int,
    foreign key (kategorija_id) references Kategorija(kategorija_id)
);
create table Racun (
    raucn_id int primary key identity(1,1),
    datum datetime,
    cena decimal(10, 2),
	osoba_id int,
    foreign key (osoba_id) references Osoba(osoba_id)
);

insert into Kategorija (naziv) 
values
('Povrce i voce'),
('Meso i mesni proizvodi'),
('Mlecni proizvodi'),
('Pekara'),
('Pice'),
('Slatkisi'),
('Bogatstvo hrane'),
('Higijena i sredstva za ciscenje'),
('Zdravlje i lepota');

insert into Proizvod (naziv, cena, kolicina, barkod, kategorija_id) 
values
('Jabuka', 80, 100, '123456789012', 1),
('Krompir', 50, 150, '987654321098', 1),
('Banane', 90, 80, '234567890123', 1),
('Pileca prsa', 250, 50, '345678901234', 2),
('Kravlje mleko', 120, 200, '456789012345', 3),
('Kifla', 20, 300, '567890123456', 4),
('Pogaca', 25, 150, '678901234567', 4),
('Koka Kola', 150, 120, '789012345678', 5),
('Fanta', 140, 130, '890123456789', 5),
('Cokolada', 100, 80, '901234567890', 6),
('Torta', 350, 30, '012345678901', 6),
('Pirinac', 120, 200, '123456789123', 7),
('Testenina', 130, 200, '234567890234', 7),
('Deterdzent', 250, 50, '345678901345', 8),
('Sredstvo za ciscenje', 300, 40, '456789012456', 8),
('Krema za lice', 350, 30, '567890123567', 9),
('Sampon', 150, 50, '678901234678', 9);

insert into Osoba (ime, prezime, jmbg, telefon, username, sifra, uloga) 
values
('Marko', 'Markovic', '1234567890123', '0601234567','Markoo','admin123', 'administrator'),
('Jovana', 'Jovanovic', '2345678901234', '0612345678', 'Jovana12','prodavac123', 'prodavac'),
('Ana', 'Anic', '3456789012345', '0623456789', 'Anica','admin456', 'administrator'),
('Nikola', 'Nikolic', '4567890123456', '0634567890', 'NikolicN','prodavac456', 'prodavac');


