CREATE PROCEDURE dbo.[Params_GetTimeTable]
(
    @hr_from int = 0,
    @hr_to int = 24,
    @min_interval int = 15
)
AS
BEGIN
-- Internal Variables
declare @hr int = @hr_from
declare @min int = 0
declare @timetable table
(
    hhmm varchar(5)
)

-- Populate the @timetable
    while @hr < @hr_to
    begin
        while @min < 60
        begin
            insert into @timetable(hhmm)
            select 
                case 
                    when @hr < 10 then '0' + cast(@hr as varchar(2)) + ':' + case when @min < 10 then '0' + cast(@min as varchar(2)) else cast(@min as varchar(2)) end
                    else cast(@hr as varchar(2)) + ':' + case when @min < 10 then '0' + cast(@min as varchar(2)) else cast(@min as varchar(2)) end
                end
            set @min = @min + @min_interval
        end
        set @hr = @hr + 1
        set @min = 0
    end

--  Add a finishing time to the output table
    insert into @timetable(hhmm)
    select 
        case 
            when @hr < 10 then '0' + cast(@hr as varchar(2)) + ':00'
            else cast(@hr as varchar(2)) + ':00'
        end

-- Return the output
 select hhmm from @timetable

END

EXEC dbo.[Params_GetTimeTable] 0, 24, 15