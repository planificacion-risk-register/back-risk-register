create database riskPlan


create table projects (
id int primary key identity,
label varchar(50)
);

CREATE SEQUENCE task_sequence
    START WITH 1
    INCREMENT BY 1;

create table task_register(
id_plan int primary key identity,
id_project int,
id_task int,
task_name varchar (100)
foreign key (id_project) references projects(id)
)

create table owner (
id int primary key identity,
label varchar (25)
)


create table risks (
id_risk int primary key identity,
id_plan int,
risk_description varchar(50),
impact_description varchar (50),
impact varchar (2),
probability varchar (2),
owner varchar (2),
response_plan varchar (50),
priority varchar (2),
foreign key (id_plan) references task_register (id_plan)
)

select * from risks

SELECT IDENT_CURRENT('risk_register') AS lastId;