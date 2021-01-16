insert tblKlijent(
        Ime,
        Prezime,
        Grad,
        Adresa,
        TelefonskiBroj,
        KorisnickoIme,
        KorisnickaLozinka
    )
values (
        'Boban',
        N'Zlatić',
        'Beograd',
        'Nikole Tesle 4/43',
        '+381623875098',
        'bzlatic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Marko',
        N'Marković',
        'Novi Sad',
        N'Zorana Đinđića 1',
        '+381640987376',
        'mmarkovic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Igor',
        'Petrović',
        'Novi Sad',
        N'Danila Kiša 53b',
        '+381609876555',
        'ipetrovic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Eva',
        N'Ostojić',
        'Beograd',
        'Svetog Save 12/4',
        '+381618800987',
        'mostojic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Ivo',
        N'Aleksandrić',
        'Sremska Mitrovica',
        N'Njegoševa 13',
        '+381659840387',
        'ivoaleksandric',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Isidora',
        N'Leposavić',
        'Sremska Mitrovica',
		N'Njegoševa 13',
        '+381694300435',
        'ileposavic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    )
insert tblMenadzer(
        Ime,
        Prezime,
        Lokacija,
        TelefonskiBroj,
        KorisnickoIme,
        KorisnickaLozinka
    )
values (
        'Kurirska',
        N'Služba',
        'Beograd',
        '+381623875098',
        'admin',
        'c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec'
    ),
    (
        'Mladen',
        N'Draganović',
        'Novi Sad',
        '+381640987376',
        'draganovik',
        '9faf1d70c13db5f4777a860c6470304b8307590f6130fadbc627f9be372db776ab2f5ab12244862c85cff329b03b7cd5ddb4cda82b0f740a70891006c0625432'
    ),
	(
        'Stefan',
        N'Hadžiruminov',
        'Beograd',
        '+381698877098',
        'shadziruminov',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    )
insert tblKurir(
        Ime,
        Prezime,
        Lokacija,
        TelefonskiBroj,
        KorisnickoIme,
        KorisnickaLozinka
    )
values (
        'Stefan',
        'Ivanov',
        'Beograd',
        '+381694455389',
        'sivanov',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Petra',
        'Ratkov',
        'Novi Sad',
        '+381612233456',
        'pratkov',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Teodora',
        N'Stefanović',
        'Novi Sad',
        '+381698803612',
        'tstefanovic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Lana',
        N'Teodorović',
        'Novi Sad',
        '+381647699032',
        'lteodorovic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Ivan',
        N'Aleksić',
        'Novi Sad',
        '+381658877011',
        'ialeksic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    ),
    (
        'Nikola',
        N'Tešić',
        'Beograd',
        '+381649901133',
        'ntesic',
        '9af7108eee30db682ac0b879a40402997031cb424913e1b8df1db64c9eeb06176dffa8d88dccc96f89e9dd28fc71dcde65a048dc194b6b5710d6a6a227cfb1bc'
    )
insert tblCenovnik
values ('Gradska dostava', 249),
    (N'Međugradska dostava', 349),
    ('Brza dostava', 499),
	('Besplatna dostava', 0),
	('Specijalna dostava', 199)
insert tblVrstaStanjaPosiljke(NazivStanja)
values (N'NEOBRAĐENA'),
    ('OBRADA U TOKU'),
    ('PREUZIMA SE'),
    ('U TRANSPORTU'),
	(N'URUČUJE SE'),
	(N'ISPORUČENA'),
	('OTKAZANA')
insert tblStanjeDoplateZaPaket(NazivStanja)
values ('NEMA DOPLATE'),
    (N'NIJE NAPLAĆENO'),
    (N'NAPLAĆENO'),
    (N'ISPLAĆENO'),
    (N'VRAĆENO')
insert tblPosiljka (
        Naziv,
        Tezina,
        DodeljenMenadzerID,
        DodeljenPosiljalacID,
        DodeljenPrimalacID,
        GradPreuzimanja,
        AdresaPreuzimanja,
        GradDostave,
        AdresaDostave,
		DodeljenKurirID,
		PostarinaID,
		DoplataZaPaket,
        VremeDostave
        
    )
values (
        'iPhone 12 Pro Max',
        '800',
        2,
        1,
        2,
        'Beograd',
        'Nikole Tesle 4/43',
        'Novi Sad',
       N'Zorana Đinđića 1',
		1,
		2,
		80000,
        '2021-02-01'
    ),
    (
        N'Rimski novčić iz I ere',
        '400',
        2,
        2,
        3,
        'Novi Sad',
        N'Zorana Đinđića 1',
        'Novi Sad',
        N'Danila Kiša 53b',
		2,
		1,
		4800,
        '2021-02-02'
    ),
    (
        'Kosilica Villiger R45',
        '6000',
        2,
        3,
        1,
        'Novi Sad',
        N'Danila Kiša 53b',
        'Beograd',
        'Nikole Tesle 4/43',
		2,
		3,
		3899,
        '2021-02-03'
    ),
	(
        'Zaštitno staklo za iPhone',
        '100',
        2,
        1,
        2,
        'Beograd',
        'Nikole Tesle 4/43',
        'Novi Sad',
        N'Zorana Đinđića 1',
		1,
		2,
		1200,
        '2021-02-01'
    ),
	(
        'Xbox Series S + Halo Infinite',
        '2000',
        2,
        3,
        2,
        'Novi Sad',
        N'Danila Kiša 53b',
        'Novi Sad',
        N'Zorana Đinđića 1',
		1,
		2,
		4000,
        '2021-02-06'
    )
insert tblStanjePosiljke(PosiljkaID, VrstaStanjaID, Komentar, Vreme)
values (1, 1, 'Nema komentara', '2021-01-15 23:47:07.000'),
(2, 1, 'Nema komentara', '2021-01-15 23:47:07.000'),
(3, 1, 'Nema komentara', '2021-01-15 23:47:07.000'),
(4, 1, 'Nema komentara', '2021-01-15 23:47:07.000'),
(5, 1, 'Nema komentara', '2021-01-15 23:47:07.000'),
(1, 2, 'Početa obrada', '2021-01-15 23:50:10.000')
