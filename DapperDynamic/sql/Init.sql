drop database if exists dapperdynamic;
create database dapperdynamic;
use dapperdynamic;
create table usertables(
    tablerealname varchar(64) primary key,
    tabledisplayname varchar(64) not null unique);
create table csharpmap(
    csharptype varchar(16) primary key);
create table usertablescolumns(
                              realname varchar(64) primary key,
                              displayname varchar(64) not null unique,
                              tablerealname varchar(64) not null,
                              csharptype varchar(16) not null,
                              color char(6) not null,
                              constraint fk_utc_ut foreign key(tablerealname) references usertables(tablerealname),
                              constraint fk_utc_csm foreign key(csharptype) references csharpmap(csharptype));

insert into csharpmap values('INT'), ('FLOAT'), ('DOUBLE'), ('DECIMAL'), ('BOOL'), ('CHAR'), ('STRING');