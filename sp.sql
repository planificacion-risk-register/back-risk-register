CREATE or alter PROCEDURE add_risk
(
    @id_plan tinyint,
    @risk_description VARCHAR(50),
    @impact_description VARCHAR(50),
    @impact varchar(2),
    @probability varchar(2),
    @owner varchar(2),
    @response_plan VARCHAR(50),
    @priority varchar(2)
)
AS
BEGIN
    INSERT INTO risks (id_plan, risk_description, impact_description, impact, probability, owner, response_plan, priority)
    VALUES (@id_plan, @risk_description, @impact_description, @impact, @probability, @owner, @response_plan, @priority);
END

CREATE or alter PROCEDURE update_risk
(
    @id_risk tinyint,
    @risk_description VARCHAR(50),
    @impact_description VARCHAR(50),
    @impact varchar(2),
    @probability varchar(2),
    @owner varchar(2),
    @response_plan VARCHAR(50),
    @priority varchar(2)
)
AS
BEGIN
    UPDATE risks SET risk_description = @risk_description, impact_description = @impact_description,
                  impact = @impact,
                  probability = @probability,
                  owner = @owner,
                  response_plan = @response_plan,
                  priority = @priority
              WHERE id_risk = @id_risk
END

create or alter procedure last_risk_register_id 
as begin 
SELECT IDENT_CURRENT('task_register') AS lastId
end

exec last_risk_register_id
CREATE or alter PROCEDURE delete_risk
(
    @id_risk tinyint
)
AS
BEGIN
    DELETE FROM risks WHERE id_risk = @id_risk;
END

CREATE PROCEDURE get_risks_by_id_plan
(
    @id_plan tinyint
)
AS
BEGIN
    SELECT * FROM risks WHERE id_plan = @id_plan;
END

select * from task_register