CREATE PROCEDURE addPlan
    @idProject INT,
    @taskName VARCHAR(100),
    @lastUpdate DATE,
    @riskCount TINYINT,
    @totalPoints TINYINT
AS
BEGIN
    INSERT INTO task_register (id_project, id_task, task_name, last_update, risk_count, total_points, enabled)
    VALUES (@idProject, NEXT VALUE FOR task_sequence, @taskName, @lastUpdate, @riskCount, @totalPoints, 'false');
END

CREATE PROCEDURE UpdatePlan
    @idProject INT,
    @taskName VARCHAR(100),
    @lastUpdate DATE,
    @riskCount TINYINT,
    @totalPoints TINYINT,
    @idPlan INT
AS
BEGIN
    UPDATE task_register
    SET id_project = @idProject,
        task_name = @taskName,
        last_update = @lastUpdate,
        risk_count = @riskCount,
        total_points = @totalPoints
    WHERE id_plan = @idPlan;
END

CREATE PROCEDURE DeletePlan
    @idPlan INT
AS
BEGIN
    UPDATE task_register
    SET enabled = 1
    WHERE id_plan = @idPlan;
END


create procedure getAllPlan 
as 
begin 
SELECT * FROM task_register where enabled=false
end