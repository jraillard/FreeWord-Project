CREATE TABLE Profile 
(
	usr_id int AUTO_INCREMENT, 
	usr_name varchar(32) not null,
	usr_pwd varchar(32) not null, 
	usr_gameLangage varchar(32) not null, 
	
	CONSTRAINT pk_usrId primary key(usr_id),
	CONSTRAINT chk_gameLangage check(usr_gameLangage IN ('English', 'Français'))
	
);

CREATE TABLE Choice 
( 
    chc_id int AUTO_INCREMENT,
    chc_usr int not null, 
    chc_langage int not null, 
    
    CONSTRAINT pk_chc primary key(chc_id),
    CONSTRAINT fk_chc1 foreign key(chc_usr) references profile(usr_id),
    CONSTRAINT fk_chc2 foreign key(chc_langage) references langage(lng_id)
);

CREATE TABLE Langage 
(
	lng_id int AUTO_INCREMENT,
	lng_langage varchar(32) not null, 
	
	CONSTRAINT pk_lngId primary key(lng_id),
	CONSTRAINT chk_Langage check(lng_langage IN ('English', 'Français'))
	
);

CREATE TABLE Category 
( 
	cat_id int AUTO_INCREMENT, 
	cat_name varchar(32) not null, 
	cat_reached boolean not null, /* all word discover or not */
	cat_lng int not null,
	
	CONSTRAINT pk_catId primary key(cat_id),
	CONSTRAINT fk_catId foreign key(cat_lng) references Langage(lng_id)

);

CREATE TABLE Word 
( 
	wd_id int AUTO_INCREMENT, 
	wd_value varchar(30) not null, /* the word */
	wd_nbtime int not null, /* store the number that the user */
	wd_cat int not null,
    	
	
	CONSTRAINT pk_wdId primary key(wd_id),
	CONSTRAINT fk_wdId foreign key(wd_cat) references Category(cat_id)
	
);

/*Don't need anymore Level = NbwordsFound-5
CREATE TABLE Level 
(
	lvl_id int AUTO_INCREMENT,
	lvl_num integer(3) not null, 
	lvl_reached boolean not null,
	lvl_cat int not null,
	
	CONSTRAINT pk_lvlId primary key(lvl_id),
	CONSTRAINT fk_lvlId foreign key(lvl_cat) references Category(cat_id)
	
);
*/