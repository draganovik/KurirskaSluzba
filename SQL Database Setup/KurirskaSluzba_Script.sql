USE [master]
GO
/****** Object:  Database [SistemKurirskeSluzbe]    Script Date: 17/01/2021 00:05:38 ******/
CREATE DATABASE [SistemKurirskeSluzbe]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SistemKurirskeSluzbe', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SistemKurirskeSluzbe.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SistemKurirskeSluzbe_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\SistemKurirskeSluzbe_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SistemKurirskeSluzbe].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ARITHABORT OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET RECOVERY FULL 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET  MULTI_USER 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SistemKurirskeSluzbe', N'ON'
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET QUERY_STORE = OFF
GO
USE [SistemKurirskeSluzbe]
GO
/****** Object:  Table [dbo].[tblCenovnik]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCenovnik](
	[CenaID] [int] IDENTITY(1,1) NOT NULL,
	[Opis] [nvarchar](30) NOT NULL,
	[Cena] [money] NOT NULL,
 CONSTRAINT [PK_tblCenovnik] PRIMARY KEY CLUSTERED 
(
	[CenaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblKlijent]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblKlijent](
	[KlijentID] [int] IDENTITY(1,1) NOT NULL,
	[Ime] [nvarchar](16) NOT NULL,
	[Prezime] [nvarchar](16) NOT NULL,
	[TelefonskiBroj] [varchar](16) NOT NULL,
	[Grad] [nvarchar](20) NULL,
	[Adresa] [nvarchar](50) NULL,
	[KorisnickaLozinka] [nvarchar](128) NOT NULL,
	[KorisnickoIme] [varchar](16) NOT NULL,
 CONSTRAINT [PK_tblKlijent] PRIMARY KEY CLUSTERED 
(
	[KlijentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblKurir]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblKurir](
	[KurirID] [int] IDENTITY(1,1) NOT NULL,
	[Ime] [nvarchar](16) NOT NULL,
	[Prezime] [nvarchar](16) NOT NULL,
	[TelefonskiBroj] [varchar](16) NOT NULL,
	[Lokacija] [nvarchar](50) NULL,
	[KorisnickaLozinka] [nvarchar](128) NOT NULL,
	[KorisnickoIme] [varchar](16) NOT NULL,
 CONSTRAINT [PK_tblKurir] PRIMARY KEY CLUSTERED 
(
	[KurirID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMenadzer]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMenadzer](
	[MenadzerID] [int] IDENTITY(1,1) NOT NULL,
	[Ime] [nvarchar](16) NOT NULL,
	[Prezime] [nvarchar](16) NOT NULL,
	[TelefonskiBroj] [varchar](16) NOT NULL,
	[Lokacija] [nvarchar](50) NULL,
	[KorisnickaLozinka] [nvarchar](128) NOT NULL,
	[KorisnickoIme] [varchar](16) NOT NULL,
 CONSTRAINT [PK_tblMenadzer] PRIMARY KEY CLUSTERED 
(
	[MenadzerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPosiljka]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPosiljka](
	[PosiljkaID] [int] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](30) NULL,
	[Tezina] [int] NULL,
	[DodeljenMenadzerID] [int] NOT NULL,
	[DodeljenKurirID] [int] NOT NULL,
	[DodeljenPosiljalacID] [int] NOT NULL,
	[DodeljenPrimalacID] [int] NOT NULL,
	[GradPreuzimanja] [nvarchar](20) NOT NULL,
	[AdresaPreuzimanja] [nvarchar](50) NOT NULL,
	[GradDostave] [nvarchar](20) NOT NULL,
	[AdresaDostave] [nvarchar](50) NOT NULL,
	[PostarinaID] [int] NOT NULL,
	[DoplataZaPaket] [money] NULL,
	[StanjeDoplateZaPaket] [int] NULL,
	[VremeDostave] [datetime] NULL,
	[Napomena] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblPosiljka] PRIMARY KEY CLUSTERED 
(
	[PosiljkaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblStanjeDoplateZaPaket]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStanjeDoplateZaPaket](
	[StanjeDoplateZaPaketID] [int] IDENTITY(1,1) NOT NULL,
	[NazivStanja] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblStanjeDoplateZaPaket] PRIMARY KEY CLUSTERED 
(
	[StanjeDoplateZaPaketID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblStanjePosiljke]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblStanjePosiljke](
	[StanjePosiljkeID] [int] IDENTITY(1,1) NOT NULL,
	[PosiljkaID] [int] NOT NULL,
	[VrstaStanjaID] [int] NOT NULL,
	[Komentar] [nvarchar](50) NULL,
	[Vreme] [datetime] NULL,
 CONSTRAINT [PK_tblStanjePosiljke] PRIMARY KEY CLUSTERED 
(
	[StanjePosiljkeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVrstaStanjaPosiljke]    Script Date: 17/01/2021 00:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVrstaStanjaPosiljke](
	[VrstaStanjaID] [int] IDENTITY(1,1) NOT NULL,
	[NazivStanja] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblVrstaStanjaPosiljke] PRIMARY KEY CLUSTERED 
(
	[VrstaStanjaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblPosiljka] ADD  CONSTRAINT [DF_tblPosiljka_Postarina]  DEFAULT ((1)) FOR [PostarinaID]
GO
ALTER TABLE [dbo].[tblPosiljka] ADD  CONSTRAINT [DF_tblPosiljka_DoplataZaPaket]  DEFAULT ((0)) FOR [DoplataZaPaket]
GO
ALTER TABLE [dbo].[tblPosiljka] ADD  CONSTRAINT [DF_tblPosiljka_StanjeDoplateZaPaket]  DEFAULT ((1)) FOR [StanjeDoplateZaPaket]
GO
ALTER TABLE [dbo].[tblStanjePosiljke] ADD  CONSTRAINT [DF_tblStanjePosiljke_Vreme]  DEFAULT (getdate()) FOR [Vreme]
GO
ALTER TABLE [dbo].[tblPosiljka]  WITH CHECK ADD  CONSTRAINT [FK_DodeljenKurir] FOREIGN KEY([DodeljenKurirID])
REFERENCES [dbo].[tblKurir] ([KurirID])
GO
ALTER TABLE [dbo].[tblPosiljka] CHECK CONSTRAINT [FK_DodeljenKurir]
GO
ALTER TABLE [dbo].[tblPosiljka]  WITH CHECK ADD  CONSTRAINT [FK_DodeljenMenadzer] FOREIGN KEY([DodeljenMenadzerID])
REFERENCES [dbo].[tblMenadzer] ([MenadzerID])
GO
ALTER TABLE [dbo].[tblPosiljka] CHECK CONSTRAINT [FK_DodeljenMenadzer]
GO
ALTER TABLE [dbo].[tblPosiljka]  WITH CHECK ADD  CONSTRAINT [FK_DodeljenPosiljalac] FOREIGN KEY([DodeljenPosiljalacID])
REFERENCES [dbo].[tblKlijent] ([KlijentID])
GO
ALTER TABLE [dbo].[tblPosiljka] CHECK CONSTRAINT [FK_DodeljenPosiljalac]
GO
ALTER TABLE [dbo].[tblPosiljka]  WITH CHECK ADD  CONSTRAINT [FK_DodeljenPrimalac] FOREIGN KEY([DodeljenPrimalacID])
REFERENCES [dbo].[tblKlijent] ([KlijentID])
GO
ALTER TABLE [dbo].[tblPosiljka] CHECK CONSTRAINT [FK_DodeljenPrimalac]
GO
ALTER TABLE [dbo].[tblPosiljka]  WITH CHECK ADD  CONSTRAINT [FK_Postarina] FOREIGN KEY([PostarinaID])
REFERENCES [dbo].[tblCenovnik] ([CenaID])
GO
ALTER TABLE [dbo].[tblPosiljka] CHECK CONSTRAINT [FK_Postarina]
GO
ALTER TABLE [dbo].[tblPosiljka]  WITH CHECK ADD  CONSTRAINT [FK_StanjeDoplateZaPaket] FOREIGN KEY([StanjeDoplateZaPaket])
REFERENCES [dbo].[tblStanjeDoplateZaPaket] ([StanjeDoplateZaPaketID])
GO
ALTER TABLE [dbo].[tblPosiljka] CHECK CONSTRAINT [FK_StanjeDoplateZaPaket]
GO
ALTER TABLE [dbo].[tblStanjePosiljke]  WITH CHECK ADD  CONSTRAINT [FK_Posiljka] FOREIGN KEY([PosiljkaID])
REFERENCES [dbo].[tblPosiljka] ([PosiljkaID])
GO
ALTER TABLE [dbo].[tblStanjePosiljke] CHECK CONSTRAINT [FK_Posiljka]
GO
ALTER TABLE [dbo].[tblStanjePosiljke]  WITH CHECK ADD  CONSTRAINT [FK_VrstaStanjePosiljke] FOREIGN KEY([VrstaStanjaID])
REFERENCES [dbo].[tblVrstaStanjaPosiljke] ([VrstaStanjaID])
GO
ALTER TABLE [dbo].[tblStanjePosiljke] CHECK CONSTRAINT [FK_VrstaStanjePosiljke]
GO
USE [master]
GO
ALTER DATABASE [SistemKurirskeSluzbe] SET  READ_WRITE 
GO
