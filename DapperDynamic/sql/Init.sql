drop database if exists dapperdynamic;
create database dapperdynamic;
use dapperdynamic;
create table usertables(
    tablename varchar(64) primary key);
create table csharpmap(
    csharptype varchar(16) primary key);
create table usertablescolumns(
                                  colname varchar(64) primary key,
                                  tablename varchar(64) not null,
                                  csharptype varchar(16) not null,
                                  color char(6) not null,
                                  constraint fk_utc_ut foreign key(tablename) references usertables(tablename),
                                  constraint fk_utc_csm foreign key(csharptype) references csharpmap(csharptype));

insert into csharpmap values('INT'), ('FLOAT'), ('DOUBLE'), ('DECIMAL'), ('BOOL'), ('CHAR'), ('STRING');