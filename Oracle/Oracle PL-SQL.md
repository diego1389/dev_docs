## Introduction

- The PL/SQL block is send to the PL/SQL Engine (inside the DB server or client tool).
- The engine processes the procedural code and send it to the database engine (SQL Engine).

## Anonymous blocks

- Most basic unit in PL/SQL. Combine SQL statements and use them as block. 
- Use for one off executions.

    ```SQL
    DECLARE
    l_counter NUMBER;

    BEGIN
    l_counter := 1;
    dbms_output.put_line('Hello ' || l_counter);
    EXCEPTION
    WHEN OTHERS THEN
        null;
    END;
    ```
    - In the declaration section you declare all the variables to be used during your block.
    - All lines have to end with a ; except for code blocks and other elements.
    - **:=** assignment operator vs **=** which is a compare operator. 
    - When others is a catch all exception block. 
    - The declaration and exception sections are optional.
- **--** for single line comments, /* */ for multi-line comments.
- PL/SQL is not case sensitive. 
- **||** Concatenation symbol.
- To see printed messages go to View -> Dbms output -> select the connection.
- You can nest anonymous blocks. 
- Variables declared under the inner block have a limited scope inside that inner block.
- If you declare two variables with the same name in an outer block and in an inner block, the variable in the inner block will override the variable from the outer block inside its scope.

    ```SQL
    <<parent>>
    DECLARE
    l_var NUMBER;

    BEGIN
    l_var := 1;
    dbms_output.put_line('Parent block ' || l_var);
        DECLARE
        l_var NUMBER;
        BEGIN
        l_var := 2;
        dbms_output.put_line('Parent block ' || parent.l_var || ' inner block: ' || l_var);
        END;
    EXCEPTION
    WHEN OTHERS THEN
        null;
    END;
    /*
    Parent block 1
    Parent block 1 inner block: 2
    */
    ```
    * Add a **block** label to access specific block variables. 

## Commonly used data types

1. Scalar datatypes.
    - Numeric:
        - Number.
            - Most commonly used.
            - Portable across platform.
            - Vast amount of values. 
            - Number(precision, scale). 
            - Negative scale causes numbers to round in the opposite direction of positive numbers. 
                - For example: Number(5, -2). 21345.678 => 21300.
            - When we do not explicitly declare the precision and the scale NUMBER represents any floating number. 
        - PLS_Integer/Binary_Integer.
        - Binary_float/Binary_Double.
    - 
2. Composite datatypes (records and collections).
3. Reference datatypes (cursors).
4. Other datatypes (BLOBS, etc).

- If variable is not assigned it will not have a default value.
- Constants 
    ```SQL
    DECLARE
    l_second CONSTANT NUMBER DEFAULT 2.1;
    l_first NUMBER;
    BEGIN
    l_first := 5;
    l_second :=3; --Error
    dbms_output.put_line(l_second);
    END;
    /
    ```
- Subtypes: 
    - Integer is a subtype of number = NUMBER(0,38);
    - Dec, decimal, numeric = fixed point 38 digits.
    - Double presicion, float, real = floating point 38 digits.
    - Integer, smallint, int = Integer, 38 digits.
    - If you assign a decimal value to an integer subtype it will round it.
- **%Type**: anchor type. A variable declared with an anchor type takes the datatype of its anchor.
    ```SQL
    DECLARE 
    l_num NUMBER(5,2) NOT NULL DEFAULT 2.1;
    l_num_vartype l_num%TYPE := 1.123;

    BEGIN
    dbms_output.put_line(l_num_vartype); --1,12
    END;
    ```
    - If you anchor the variable datatype to a column, it only inherits the datatype but not the constraints.
    - The variable type also adapt to any changes on the database table column types.
- Binary_double / Binary_float does not raise overflow /underflow exceptions (check Binary_double_nan, binary_double_infinite, etc).
- TO_NUMBER built-in function to cast to a number.
- Every database is stored with a database character set (CHAR, VARCHAR, CLOB, etc).
- National character set (NCHAR, NVARCHAR, NVARCHAR2). To store french characters for example.
- CHAR(4) = 'ab' gets stored as 'ab(space)(space)'.
- TO_DATE built-in function to specify the year, month, day, hour, etc.
- CURRENT_DATE returns the session date.
- SYSDATE returns the date as on the db server.
- PL/SQL boolean datatype, it only allows TRUE, FALSE AND NULL VALUES.
-  Composity datatypes:
    - **Records:** to group related variables together. (Like a row on a table). 
        - Syntax: TYPE rec_name IS RECORD (FieldDeclaration, [FieldDeclarations]).
        ```SQL
        DECLARE TYPE emp_rec IS RECORD (emp_name VARCHAR2(60),
                                    dept_id departments.dept_id%TYPE,
                                    location VARCHAR2(60) DEFAULT 'CA');
        l_emp emp_rec;
        BEGIN
        l_emp.emp_name := 'John';
        l_emp.dept_id := 10;
        DBMS_OUTPUT.PUT_LINE('Émployee name is: ' || l_emp.emp_name);
        DBMS_OUTPUT.PUT_LINE('Émployee location is: ' || l_emp.location);
        END;
        ```
        - %ROWTYPE allows us to define a record base on a view, table or cursor. 
        ```SQL
        DECLARE l_dept_rec departments%ROWTYPE;
        BEGIN
        l_dept_rec.dept_name := 'CA';
        l_dept_rec.dept_id := 10;

        DBMS_OUTPUT.PUT_LINE('Department name is: ' || l_dept_rec.dept_name);
        END;
        ```
        - You can also have a nested %ROWTYPE record:
        ```SQL
         DECLARE TYPE emp_rec IS RECORD (emp_name VARCHAR2(60),
                                    dept_rec departments%ROWTYPE,
                                    location VARCHAR2(60) DEFAULT 'CA');
        l_emp emp_rec;
        BEGIN
        l_emp.emp_name := 'John';
        l_emp.dept_rec.dept_id := 10;
        DBMS_OUTPUT.PUT_LINE('Émployee name is: ' || l_emp.emp_name);
        DBMS_OUTPUT.PUT_LINE('Department id is: ' ||  l_emp.dept_rec.dept_id );
        END;
        ```
    - Nested tables: collection types.
    - VArrays.
    - Associate Arrays.

## Loops 

- **Implicit exit:** for, while.
- **Explicit exit:** exit, exit when <codition>, return, go to.
- Simple loop:

    ```SQL
    DECLARE 
    l_counter NUMBER := 0;
    l_sum NUMBER := 0;
    BEGIN
        LOOP
        l_sum := l_counter + l_sum;
        l_counter := l_counter + 1;
        DBMS_OUTPUT.PUT_LINE('Counter: ' ||l_counter);
        EXIT WHEN l_counter >= 3;
        END LOOP;
    END;
    ```
- GOTO is used with labels. 
    ```SQL
    DECLARE 
    l_counter NUMBER := 0;
    l_sum NUMBER := 0;
    BEGIN
        LOOP
        l_sum := l_counter + l_sum;
        l_counter := l_counter + 1;
        DBMS_OUTPUT.PUT_LINE('Émployee name is: ' ||l_counter);
            GOTO out_of_loop;
        END LOOP;
    <<out_of_loop>>
    null;
    END;
    ```
- FOR loops: 
    [label] FOR loop_counter IN [reverse] lower_bound..upper_bound LOOP 
    --group of statements
    END LOOP;
    ```SQL
    DECLARE 
    l_sum NUMBER := 0;
    BEGIN
        FOR l_counter IN REVERSE 1..3
        LOOP
        l_sum := l_counter + l_sum;

        DBMS_OUTPUT.PUT_LINE('Counter: ' ||l_counter);
        END LOOP;
    END;
    ```
    - You cannot directly assign valuesto the l_counter variable.
    - The continue statement goes to the next iteration of the loop.
    ```SQL
    DECLARE 
    l_sum NUMBER := 0;
    BEGIN
        FOR l_counter IN REVERSE 1..3
        LOOP
        IF l_counter = 2 THEN
            CONTINUE;
        END IF; 
        DBMS_OUTPUT.PUT_LINE('Counter: ' ||l_counter); --prints 3, 1 

        END LOOP;
    END;
    ```
- EXIT
    ```SQL
    BEGIN
    <<outerLoop>>
    FOR l_outer_counter in 1..3 LOOP
    DBMS_OUTPUT.PUT_LINE('Outer Counter: ' ||l_outer_counter);
    <<innerLoop>>
        FOR l_inner_counter in 1..3 LOOP
            EXIT outerLoop WHEN l_outer_counter = 2;
            DBMS_OUTPUT.PUT_LINE('Inner Counter: ' ||l_inner_counter);
        END LOOP innerLoop;
    END LOOP outerLoop;
    END;
    /*
    Outer Counter: 1
    Inner Counter: 1
    Inner Counter: 2
    Inner Counter: 3
    Outer Counter: 2
    */
    ```
- CONTINUE

    ```SQL
    BEGIN
    <<outerLoop>>
    FOR l_outer_counter in 1..3 LOOP
    DBMS_OUTPUT.PUT_LINE('Outer Counter: ' ||l_outer_counter);
    <<innerLoop>>
        FOR l_inner_counter in 1..3 LOOP
            CONTINUE outerLoop WHEN l_outer_counter = 2;
            DBMS_OUTPUT.PUT_LINE('Inner Counter: ' ||l_inner_counter);
        END LOOP innerLoop;
    END LOOP outerLoop;
    END;

    /*Outer Counter: 1
    Inner Counter: 1
    Inner Counter: 2
    Inner Counter: 3
    Outer Counter: 2
    Outer Counter: 3
    Inner Counter: 1
    Inner Counter: 2
    Inner Counter: 3*/
    ```
- While loop syntax:
    ```SQL
    DECLARE 
    l_check INTEGER := 1;
    BEGIN
        WHILE l_check <= 5 LOOP
            DBMS_OUTPUT.PUT_LINE('Check value: ' ||l_check);
            l_check := l_check + 1; --1 2 3 4 5
        END LOOP;
    END;
    ```

## Conditional execution

- IF statements can optionally use brackets:

    ```SQL
    IF (l_counter = 2) OR (l_counter IS NULL) THEN
        CONTINUE;
    ELSE IF
    --Something else
    ELSE -- Optional when using ELSE IF
        EXIT;
    END IF; 
    ```
- CASE statement
    - The very first match in a case statements finishes the execution of the block (doesnt need a break)
    - The else clause is optional (if not present and doesnt match any case statement it causes a CASE_NOT_FOUND exception)

    ```SQL
    DECLARE 
    l_ticket_priority VARCHAR(200) := 'MEDIUM';
    l_support_tier NUMBER;
    BEGIN
        CASE l_ticket_priority
            WHEN 'HIGH' THEN
                l_support_tier := 1;
            WHEN 'MEDIUM' THEN
                l_support_tier := 2;
            WHEN 'LOW' THEN 
                l_support_tier := 3;
            ELSE 
                l_support_tier := 0;
        END CASE;
        DBMS_OUTPUT.PUT_LINE('l_support_tier: ' ||l_support_tier); --l_support_tier: 2
    END;
    ```
- Case expressions:
    - If one matched the others are omitted.

    ```SQL
    DECLARE 
    l_ticket_priority VARCHAR(200) := 'MEDIUM';
    l_support_tier NUMBER;
    BEGIN
        l_support_tier := 
        CASE l_ticket_priority
            WHEN 'HIGH' THEN 1
            WHEN 'MEDIUM' THEN 2
            WHEN 'LOW' THEN  3
            ELSE 0
        END;
        DBMS_OUTPUT.PUT_LINE('l_support_tier: ' ||l_support_tier);
    END;
    ```
## Cursors

- Open stage of a cursor: 
    1. Area in memory is assigned.
    2. SQL statements are parsed and binded.
    3. SQL statement executed.
    4. Pointer moved to the first row.
- Fetch stage
    1. Row fetched.
- Close stage
    1. Memory released.
    2. Closes the cursor.
- SQL%FOUND (if returns something), SQL%NOTFOUND, SQL%ROWCOUNT.
- SELECT INTO statement is an implicit cursor.
- Implicit cursor works only when one row is fetched.

```SQL
DECLARE l_dept_id DEPARTMENTS.dept_id%TYPE;
        l_dept_name DEPARTMENTS.dept_name%TYPE;
BEGIN
    SELECT dept_id, dept_name
    INTO l_dept_id, l_dept_name
    FROM DEPARTMENTS
    WHERE dept_id = 1;
    IF ( SQL%FOUND) THEN
    DBMS_OUTPUT.PUT_LINE(l_dept_id || ' ' || l_dept_name);
    END IF; 
END;
```
- Commit save the changes the data.
- Rollback rolls the operation back.
- Explicit cursor:
    ```SQL
    DECLARE l_dept_id DEPARTMENTS.dept_id%TYPE;
        l_dept_name DEPARTMENTS.dept_name%TYPE;
        CURSOR cur_get_departments IS 
           SELECT dept_id, dept_name
            INTO l_dept_id, l_dept_name
            FROM DEPARTMENTS
            WHERE dept_id = 1;
    BEGIN
        OPEN cur_get_departments;
        FETCH cur_get_departments
        INTO l_dept_id, l_dept_name;

        DBMS_OUTPUT.PUT_LINE(l_dept_id || ' ' || l_dept_name);
        CLOSE cur_get_departments;
    END;
    ```
- <cursor>.%ROWTYPE gets one variable of the type of the cursor select statement.
    ```SQL
    DECLARE 
        CURSOR cur_get_departments IS 
           SELECT dept_id, dept_name
            FROM DEPARTMENTS
            WHERE dept_id = 1;
        cur_get_departments_var cur_get_departments%ROWTYPE;
    BEGIN
        OPEN cur_get_departments;
        LOOP
            FETCH cur_get_departments
            INTO cur_get_departments_var;
            EXIT WHEN cur_get_departments%NOTFOUND;
            DBMS_OUTPUT.PUT_LINE(cur_get_departments_var.dept_id || ' ' || cur_get_departments_var.dept_name);
        END LOOP;
        CLOSE cur_get_departments;
    END;
    ```
- Pass parameters to cursor and ROWNUMBER functionality:

    ```SQL
    DECLARE 
            CURSOR cur_get_departments(p_rows NUMBER DEFAULT 1) IS 
            SELECT dept_id, dept_name
                FROM DEPARTMENTS
                WHERE ROWNUM <= p_rows;
            cur_get_departments_var cur_get_departments%ROWTYPE;
    BEGIN
        OPEN cur_get_departments;
        LOOP
            FETCH cur_get_departments
            INTO cur_get_departments_var;
            EXIT WHEN cur_get_departments%NOTFOUND;
            DBMS_OUTPUT.PUT_LINE(cur_get_departments_var.dept_id || ' ' || cur_get_departments_var.dept_name);
        END LOOP;
        CLOSE cur_get_departments;
    END;
    ```
- Cursor For Loop
    - Implicit Cursor%ROWTYPE Record variable
    - Compact
    ```SQL
    DECLARE 
        CURSOR cur_get_departments(p_rows NUMBER DEFAULT 1) IS 
           SELECT dept_id, dept_name
            FROM DEPARTMENTS
            WHERE ROWNUM <= p_rows;
    BEGIN
        FOR cur_get_departments_var IN cur_get_departments LOOP
            DBMS_OUTPUT.PUT_LINE(cur_get_departments_var.dept_id || ' ' || cur_get_departments_var.dept_name);
        END LOOP;

    END;
    ```
- If your dealing with huge amount of data you should use bulk collect and forall.
- Cursor For update creates an exclusive lock.
- NOWAIT prevents waiting if the table is locked.
    ```SQL
    DECLARE 
        CURSOR cur_move_emp (p_emp_loc employee.emp_loc%TYPE) IS
        SELECT emp_id, dept_name
        FROM DEPARTMENTS, EMPLOYEE
        WHERE emp_dept_id = dept_id
        AND emp_loc = p_emp_loc
        FOR UPDATE OF emp_loc NOWAIT;
        BEGIN
            FOR cur_move_emp_var IN cur_move_emp('CA') LOOP
                UPDATE employee 
                SET emp_loc = 'WA'
                WHERE CURRENT OF cur_move_emp; /*Ensures no one modifies the table before making the changes*/
            END LOOP;
            COMMIT; --Outside the loop!! 
        END;
    ```

- Ref cursor.
    - Multiple queries.
    - Cannot accept parameters.
    ```SQL
    DECLARE
    TYPE rc_dept IS REF CURSOR  RETURN departments%ROWTYPE;
    rc_dept_cur_initial     rc_dept;
    rc_dept_cur_final       rc_dept; 
    l_dept_rowtype          departments%ROWTYPE;
    l_choice                NUMBER := 1;
    l_lower                 NUMBER := 1;
    l_upper                 NUMBER := 2;
    BEGIN
    IF l_choice = 1 THEN
        OPEN rc_dept_cur_initial FOR
        SELECT  * FROM  departments
            WHERE  dept_id BETWEEN l_lower AND l_upper;
    ELSE 
        OPEN rc_dept_cur_initial FOR /*Open it again make it point to the new query*/
        SELECT  * FROM  departments
            WHERE  dept_name = 'Sales';
    END IF;
    rc_dept_cur_final := rc_dept_cur_initial;
    LOOP
        FETCH rc_dept_cur_final INTO l_dept_rowtype;
        EXIT WHEN rc_dept_cur_final %NOTFOUND;
        DBMS_OUTPUT.PUT_LINE(l_dept_rowtype.dept_id); --1 2
    END LOOP;
    CLOSE rc_dept_cur_final ;
    END;
    ```
- Weak ref cursor:
    - Return type not specified
    - Flexible
    - Runtime errors
    ```SQL
    DECLARE
    TYPE rc_weak IS REF CURSOR;
    rc_weak_cur rc_weak;
    l_dept_rowtype          departments%ROWTYPE;
    l_emp_rowtype          employee%ROWTYPE; 
    BEGIN
    OPEN rc_weak_cur FOR
        SELECT * FROM departments
        WHERE dept_id = 1;
        LOOP
        FETCH rc_weak_cur INTO l_dept_rowtype;
        EXIT WHEN rc_weak_cur%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE(l_dept_rowtype.dept_name);      
        END LOOP;
    OPEN rc_weak_cur FOR
        SELECT * FROM employee WHERE emp_dept_id = 2;
        LOOP
            FETCH rc_weak_cur INTO l_emp_rowtype;
            EXIT WHEN rc_weak_cur%NOTFOUND;
              DBMS_OUTPUT.PUT_LINE(l_emp_rowtype.emp_id);   
        END LOOP;
    CLOSE rc_weak_cur; 
    END;
    ```
## Exceptions

- Internally defined exceptions:
    - ORA-n syntax.
    - Implicitly raised. 
    ```SQL
    DECLARE 
    --...
    BEGIN
    --...
    EXCEPTION 
        WHEN ZERO DIVIDE THEN
        --Handle exception
        WHEN NO_DATA_FOUND THEN
        --Do something
        WHEN OTHERS THEN
        --Handle exception
    END;
    ```
    - To use them you have to assign them to a variable.
        ```SQL
        DECLARE
        l_num PLS_INTEGER;
        l_sqlcode NUMBER;
        l_sqlerrm VARCHAR2(512);
        BEGIN
        l_num := 2147483648;
        EXCEPTION
        WHEN OTHERS THEN
        l_sqlcode := SQLCODE;
        l_sqlerrm := SQLERRM;
        DBMS_OUTPUT.PUT_LINE(l_sqlcode || ': ' || l_sqlerrm);   
        END;
        ```
- User defined exceptions.
    - For BL validations.
    - Lika a variable in a declaration section but you cannot assign values.
     
     ```SQL
     DECLARE
    invalid_quantity EXCEPTION;
    l_order_qty NUMBER := -2;

    BEGIN
    IF l_order_qty < 0 THEN
    RAISE invalid_quantity;
    END IF; 
    EXCEPTION
    WHEN invalid_quantity THEN
    DBMS_OUTPUT.PUT_LINE('Invalid quantity: ' || SQLCODE || ' ' || SQLERRM); --Invalid quantity: 1 User-Defined Exception
    END;
     ```
    - Pragma EXCEPTION_INT (Associate at compile time an exception name with an errro code).
    ```SQL
    DECLARE
    l_num PLS_INTEGER;
    too_big EXCEPTION;
    PRAGMA EXCEPTION_INIT(too_big, -1426);

    BEGIN
    l_num := 214748365489;
    EXCEPTION
    WHEN too_big THEN
    DBMS_OUTPUT.PUT_LINE('Too big: ' || SQLCODE || ' ' || SQLERRM); 
    /*Too big: -1426 ORA-01426: desbordamiento numérico*/
    END;
    ```
- ## Debugging

- DBMS_OUTPUT
    - Built-in package. 
    - **PUT_LINE(msg IN VARCHAR2)**
    - **PUT(msg IN VARCHAR2)**
        - Doesn't add a end of line marker.
    - **NEW_LINE**
        - Doesn't take any parameters and adds a new line.
    - **GET_LINE(line OUT VARCHAR2, status OUT INTEGER)**
        - Gets a single line.
        - status 0 success and 1 nothing more to fetch.
- DBMS_UTILITY
    - **FORMAT_ERROR_STACK** (use it instead of SQLERRM, supports bigger error messages).
        ```SQL
        DECLARE
        --...
        BEGIN
        --...
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(DBMS_UTILITY.FORMAT_ERROR_STACK); 
        END;
        ```
    - **FORMAT_ERROR_BACKTRACE**
        - Give us the line number of error.
        ```SQL
        DECLARE
        --...
        BEGIN
        --...
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE(DBMS_UTILITY.FORMAT_ERROR_BACKTRACE);
            /*ORA-06512: en línea 7*/ 
        END;
        ```
- To local debug -> Tools -> Preferences -> Debugger -> Show debug options.
- For anonymous blogs configure Start debugging option: **Step into**.

# PL/SQL Bootcamp

- Case insensitivity.
- Enables object oriented programming.
- Schema as user of database (collection of objects for each user in Oracle Database).
- A user can only have one schema.
- Exceptions are different than compiling errors (mising semicolon f.e).
- There are three types of blocks: anonymous blocks, procedures and functions.
- Types of datatypes: Scalar, Reference, Large objects and Composite.
- Naming conventions:
    - Variable: v_variable_name
    - Cursors: cur_cursor_name or c_cursor_name
    - Exceptions: e_exception_name.
    - Procedures: p_procedure_name.
    - Bind variables: b_bind_variable_name.
- Variables:
    
    Name [CONSTANT] datatype [NOT NULL] [:= DEFAULT value|expression];
- **Bind variables:**
    - Create in a host environment. 
    - The scope is the whole worksheet.
    ```SQL
    variable var_text varchar2(30);
    
    declare v_text varchar2(30);
    begin
        :var_text := 'Hello PL/SQL';
        v_text  := :var_text;
        
        dbms_output.put_line(v_text);
    end;

    ```
    - Use them to execute a Select query:
    ```SQL
    variable emp_id number;

    begin
    :emp_id := 100;
    end;

    SELECT * FROM EMPLOYEES WHERE employee_id = :emp_id;
    ```
- False and null return false.
- True and null return null;
- You can use IN statement in Case expressions.
- Basic loop must iterate at least once (do...while).
- While loops when you dont know when it will stop.
- **Continue:**

    CONTINUE [label_name] [WHEN condition];
    - Use when instead of If to use continue.
    ```SQL
    declare
    v_inner number := 1;
    begin
    <<outer_loop>>
    for v_outer in 1..10 loop
    dbms_output.put_line('My outer value is : ' || v_outer );
        v_inner := 1;
        <<inner_loop>>
        loop
        v_inner := v_inner+1;
        continue outer_loop when v_inner = 10;
        dbms_output.put_line('  My inner value is : ' || v_inner );
        end loop inner_loop;
    end loop outer_loop;
    end;
    ```
- **GOTO statement:**
    - You cannot go to:
        - A control structure.
        - Into an inner block from an outer block.
        - Out of a subprogram.
        - In or out of an exception handler.
    - Simulate a loop using Go to statement:
    ```SQL
    DECLARE
    v_searched_number NUMBER := 32457;
    v_is_prime boolean := true;
    x number := 2;
    BEGIN
    <<start_point>>
        IF v_searched_number MOD x = 0 THEN
        dbms_output.put_line(v_searched_number|| ' is not a prime number..');
        v_is_prime := false;
        GOTO end_point;
        END IF;
    x := x+1;
    if x = v_searched_number then
    goto prime_point;
    end if;
    goto start_point;
    <<prime_point>>
    if v_is_prime then
        dbms_output.put_line(v_searched_number|| ' is a prime number..');
    end if;
    <<end_point>>
    dbms_output.put_line('Check complete..');
    END;
    ```
- You cannot create DDL or DCL with PL/SQL.
- A block is not a transaction.
- You have to have an Into keyword when writing a Select in PL/SQL.
    ```SQL
    declare
    v_name varchar2(50);
    v_salary employees.salary%type;
    v_employee_id employees.employee_id%type := 130;
    begin
    select first_name ||' '|| last_name, salary into v_name, v_salary from employees where employee_id = v_employee_id;
    dbms_output.put_line('The salary of '|| v_name || ' is : '|| v_salary );
    end;
    ```
- DML operation in PL/SQL
    ```SQL
    create table employees_copy as select * from employees;
    DECLARE
    v_employee_id pls_integer := 0;
    v_salary_increase number := 400;
    begin
    for i in 217..226 loop
            --insert into employees_copy 
        --(employee_id,first_name,last_name,email,hire_date,job_id,salary)
        --values 
        --(i, 'employee#'||i,'temp_emp','abc@xmail.com',sysdate,'IT_PROG',1000);
    --update employees_copy 
        --set salary = salary + v_salary_increase
        --where employee_id = i;
        delete from employees_copy
        where employee_id = i;
    end loop;
    end; 
    ```
- Using a sequence (nextval generates a new value every time you run it, currentval returns the current value every time).

    ```SQL
    create sequence employee_id_seq 
    start with 207
    increment by 1;
    -----------------------------
    begin
    for i in 1..10 loop
        insert into employees_copy 
        (employee_id,first_name,last_name,email,hire_date,job_id,salary)
        values 
        (employee_id_seq.nextval, 'employee#'||employee_id_seq.nextval,'temp_emp','abc@xmail.com',sysdate,'IT_PROG',1000);
    end loop;
    end; 
    ----------------------------
    declare
    v_seq_num number;
    begin
    select employee_id_seq.nextval into v_seq_num from dual;
    dbms_output.put_line(v_seq_num);
    end;
    ----------------------------
    declare
    v_seq_num number;
    begin
    select employee_id_seq.nextval into v_seq_num from employees_copy where rownum = 1;
    dbms_output.put_line(v_seq_num);
    end;
    ----------------------------
    declare
    v_seq_num number;
    begin
    v_seq_num := employee_id_seq.nextval; 
    dbms_output.put_line(v_seq_num);
    end;
    ----------------------------
    begin
    dbms_output.put_line(employee_id_seq.nextval);
    end;
    ----------------------------
    begin
    dbms_output.put_line(employee_id_seq.currval);
    end;
    ```
- Composite datatypes are designed for holding multiple values in one box. 
- *Record* only one row value.
    - Containers: like an object in OOP.
    - A single row for example.
    - %rowtype to easily create record.
    - For custom record: 

    type type_name is record (variable_name variable_type, [variable_name variable_type, ...])
    
    ```SQL
    declare
    r_emp employees%rowtype;
    begin
    select * into r_emp from employees where employee_id = '101';
    --r_emp.salary := 2000;
    dbms_output.put_line(r_emp.first_name||' '||r_emp.last_name|| ' earns '||r_emp.salary||
                        ' and hired at :' || r_emp.hire_date);
    end;
    ------------------------------
    declare
    --r_emp employees%rowtype;
    type t_emp is record (first_name varchar2(50),
                            last_name employees.last_name%type,
                            salary employees.salary%type,
                            hire_date date);
    r_emp t_emp;
    begin
    select first_name,last_name,salary,hire_date into r_emp 
            from employees where employee_id = '101';
    /* r_emp.first_name := 'Alex';
    r_emp.salary := 2000;
    r_emp.hire_date := '01-JAN-20'; */
    dbms_output.put_line(r_emp.first_name||' '||r_emp.last_name|| ' earns '||r_emp.salary||
                        ' and hired at :' || r_emp.hire_date);
    end;
    -------------------------------
    declare
    type t_edu is record (primary_school varchar2(100),
                            high_school varchar2(100),
                            university varchar2(100),
                            uni_graduate_date date
                            );
    type t_emp is record (first_name varchar2(50),
                            last_name employees.last_name%type,
                            salary employees.salary%type  NOT NULL DEFAULT 1000,
                            hire_date date,
                            dept_id employees.department_id%type,
                            department departments%rowtype,
                            education t_edu
                            );
    r_emp t_emp;
    begin
    select first_name,last_name,salary,hire_date,department_id 
            into r_emp.first_name,r_emp.last_name,r_emp.salary,r_emp.hire_date,r_emp.dept_id 
            from employees where employee_id = '146';
    select * into r_emp.department from departments where department_id = r_emp.dept_id;
    r_emp.education.high_school := 'Beverly Hills';
    r_emp.education.university := 'Oxford';
    r_emp.education.uni_graduate_date := '01-JAN-13'; 
    
    dbms_output.put_line(r_emp.first_name||' '||r_emp.last_name|| ' earns '||r_emp.salary||
                        ' and hired at :' || r_emp.hire_date);
    dbms_output.put_line('She graduated from '|| r_emp.education.university|| ' at '||  r_emp.education.uni_graduate_date);
    dbms_output.put_line('Her Department Name is : '|| r_emp.department.department_name);
    end;
    ```
    - When you have to insert a row from a table in another table and you have to modify the data you can use a recorder. Otherwise just use Insert into with select. 
    - Record must have the same structure of the table to user update and delete operators.
    - Row keyword for updating!
    ```SQL
    create table retired_employees as select * from employees where 1=2;
    select * from retired_employees;
    /
    declare
        r_emp employees%rowtype;
    begin
        select * into r_emp from employees where employee_id = 104;
    r_emp.salary := 0;
        r_emp.commission_pct := 0;
        insert into retired_employees values r_emp;
    end;
    -----------------------------------------
    declare
        r_emp employees%rowtype;
    begin
        select * into r_emp from employees where employee_id = 104;
        r_emp.salary := 10;
        r_emp.commission_pct := 0;
        --insert into retired_employees values r_emp;
        update retired_employees set row = r_emp where employee_id = 104;
    end;
    /
    delete from retired_employees;
    ```
- Collections multiple rows.
    - **VArray:** bounded (exact number of rows).
        - Index starts from 1.
        - 1 dimmension array.
        - Null by default.
        - firt(): first index value.
        - last(): last  index value.
        - count(): amount of elements in the varray.
        - exists(): if index exists returns true.
        - We cannot create and initialize at the same time.
        - You can create a type in a database (when you create it in the declare section it dies after the anonymous block finishes). Its call *schema level type* (create or replace type).
        ```SQL
        ---------------A simple working example
        Declare
        type e_list is varray(5) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob','Richard');
        for i in 1..5 loop
            dbms_output.put_line(employees(i));
        end loop;
        end;
        ---------------limit exceeding error example
        declare
        type e_list is varray(4) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob','Richard');
        for i in 1..5 loop
            dbms_output.put_line(employees(i));
        end loop;
        end;
        ---------------Subscript beyond cound error example
        declare
        type e_list is varray(5) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob');
        for i in 1..5 loop
            dbms_output.put_line(employees(i));
        end loop;
        end;
        ---------------A working count() example
        declare
        type e_list is varray(5) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob');
        for i in 1..employees.count() loop
            dbms_output.put_line(employees(i));
        end loop;
        end;
        ---------------A working first() last() functions example
        declare
        type e_list is varray(5) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob');
        for i in employees.first()..employees.last() loop
            dbms_output.put_line(employees(i));
        end loop;
        end;
        --------------- A working exists() function example
        declare
        type e_list is varray(5) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob');
        for i in 1..5 loop
            if employees.exists(i) then
            dbms_output.put_line(employees(i));
            end if;
        end loop;
        end;
        ---------------A working limit() function example
        declare
        type e_list is varray(5) of varchar2(50);
        employees e_list;
        begin
        employees := e_list('Alex','Bruce','John','Bob');
            dbms_output.put_line(employees.limit());
        end;
        --------------- A create-declare at the same time error example
        declare
        type e_list is varray(5) of varchar2(50);
        employees e_list('Alex','Bruce','John','Bob');
        begin
        -- employees := e_list('Alex','Bruce','John','Bob');
        for i in 1..5 loop
            if employees.exists(i) then
            dbms_output.put_line(employees(i));
            end if;
        end loop;
        end;
        --------------- A post insert varray example
        declare
        type e_list is varray(15) of varchar2(50);
        employees e_list := e_list();
        idx number := 1;
        begin
        for i in 100..110 loop
            employees.extend;
            select first_name into employees(idx) from employees where employee_id = i;
            idx := idx + 1;
        end loop;
        for x in 1..employees.count() loop
            dbms_output.put_line(employees(x));
        end loop;
        end;
        --------------- An example for the schema level varray types
        create type e_list is varray(15) of varchar2(50);
        /
        create or replace type e_list as varray(20) of varchar2(100);
        /
        declare
        employees e_list := e_list();
        idx number := 1;
        begin
        for i in 100..110 loop
            employees.extend;
            select first_name into employees(idx) from employees where employee_id = i;
            idx := idx + 1;
        end loop;
        for x in 1..employees.count() loop
            dbms_output.put_line(employees(x));
        end loop;
        end;
        /
        DROP TYPE E_LIST;
        ```
    - **Nested tables:** key value pairs (starts from 1 and goes one to one). Unbounded.
        - Keys can only be positive numbers.
        - Can be schema level type as well.
        - We can delete a value from the nested table.
        - Are not stored consecutively in the db.
        ```SQL
        ---------------The simple usage of nested tables
        declare
        type e_list is table of varchar2(50);
        emps e_list;
        begin
        emps := e_list('Alex','Bruce','John');
        for i in 1..emps.count() loop
            dbms_output.put_line(emps(i));
        end loop;
        end;
        ---------------Adding a new value to a nested table after the initialization
        declare
        type e_list is table of varchar2(50);
        emps e_list;
        begin
        emps := e_list('Alex','Bruce','John');
        emps.extend;
        emps(4) := 'Bob';
        for i in 1..emps.count() loop
            dbms_output.put_line(emps(i));
        end loop;
        end;
        ---------------Adding values from the tabledeclare
        type e_list is table of employees.first_name%type;
        emps e_list := e_list();
        idx pls_integer := 1;
        begin
        for x in 100 .. 110 loop
            emps.extend;
            select first_name into emps(idx) from employees where employee_id = x;
            idx := idx + 1;
        end loop;
        for i in 1..emps.count() loop
            dbms_output.put_line(emps(i));
        end loop;
        end;
        ---------------delete example
        declare
        type e_list is table of employees.first_name%type;
        emps e_list := e_list();
        idx pls_integer := 1;
        begin
        for x in 100 .. 110 loop
            emps.extend;
            select first_name into emps(idx) from employees where employee_id = x;
            idx := idx + 1;
        end loop;
        emps.delete(3);
        for i in 1..emps.count() loop
        if emps.exists(i) then 
            dbms_output.put_line(emps(i));
        end if;
        end loop;
        end;
        ```
    type type_name as table of value_data_type [NOT NULL]
        - We can store nested tables in our db tables but not associative arrays.
    - **Associative arrays:**(any number for keys, even strings).
        - Key must be unique.
        - Keys don't need to be sequential.
        - Can have scalar and record types.
        - Don't need to initialize them. 
        - **index of** to indicate the key type.
        - No need for extends method.
        - We user first and last to iterate through them (don't use count()).
        ```SQL
        ---------------The first example
        declare
        type e_list is table of employees.first_name%type index by pls_integer;
        emps e_list;
        begin
        for x in 100 .. 110 loop
            select first_name into emps(x) from employees 
            where employee_id = x ;
        end loop;
        for i in emps.first()..emps.last() loop
            dbms_output.put_line(emps(i));
        end loop;
        end;
        ---------------Error example for the select into clause
        declare
        type e_list is table of employees.first_name%type index by pls_integer;
        emps e_list;
        begin
        for x in 100 .. 110 loop
            select first_name into emps(x) from employees 
            where employee_id = x and department_id = 60;
        end loop;
        for i in emps.first()..emps.last() loop
            dbms_output.put_line(i);
        end loop;
        end;
        ---------------Error example about reaching the empty indexdeclare
        type e_list is table of employees.first_name%type index by pls_integer;
        emps e_list;
        begin
        emps(100) := 'Bob';
        emps(120) := 'Sue';
        for i in emps.first()..emps.last() loop
            dbms_output.put_line(emps(i));
        end loop;
        end;
        ---------------An example of iterating in associative arrays with while loops
        declare
        type e_list is table of employees.first_name%type index by pls_integer;
        emps e_list;
        idx pls_integer;
        begin
        emps(100) := 'Bob';
        emps(120) := 'Sue';
        idx := emps.first;
        while idx is not null  loop
            dbms_output.put_line(emps(idx));
            idx := emps.next(idx);
        end loop;
        end;
        ---------------An example of using string based indexes with associative arrays
        declare
        type e_list is table of employees.first_name%type index by employees.email%type;
        emps e_list;
        idx employees.email%type;
        v_email employees.email%type;
        v_first_name employees.first_name%type;
        begin
            for x in 100 .. 110 loop
            select first_name,email into v_first_name,v_email from employees
            where employee_id = x;
            emps(v_email) := v_first_name;
        end loop;
        idx := emps.first;
        while idx is not null  loop
            dbms_output.put_line('The email of '|| emps(idx) ||' is : '|| idx);
            idx := emps.next(idx);
        end loop;
        end;
        ---------------An example of using associative arrays with records
        declare
        type e_list is table of employees%rowtype index by employees.email%type;
        emps e_list;
        idx employees.email%type;
        begin
            for x in 100 .. 110 loop
            select * into emps(x) from employees
            where employee_id = x;
        end loop;
        idx := emps.first;
        while idx is not null  loop
            dbms_output.put_line('The email of '|| emps(idx).first_name 
                ||' '||emps(idx).last_name||' is : '|| emps(idx).email);
            idx := emps.next(idx);
        end loop;
        end;
        ---------------An example of using associative arrays with record types
        declare
        type e_type is record (first_name employees.first_name%type,
                                last_name employees.last_name%type,
                                email employees.email%type);
        type e_list is table of e_type index by employees.email%type;
        emps e_list;
        idx employees.email%type;
        begin
            for x in 100 .. 110 loop
            select first_name,last_name,email into emps(x) from employees
            where employee_id = x;
        end loop;
        idx := emps.first;
        while idx is not null  loop
            dbms_output.put_line('The email of '|| emps(idx).first_name 
                ||' '||emps(idx).last_name||' is : '|| emps(idx).email);
            idx := emps.next(idx);
        end loop;
        end;
        ---------------An example of printing from the last to the first
        declare
        type e_type is record (first_name employees.first_name%type,
                                last_name employees.last_name%type,
                                email employees.email%type);
        type e_list is table of e_type index by employees.email%type;
        emps e_list;
        idx employees.email%type;
        begin
            for x in 100 .. 110 loop
            select first_name,last_name,email into emps(x) from employees
            where employee_id = x;
        end loop;
        --emps.delete(100,104);
        idx := emps.last;
        while idx is not null  loop
            dbms_output.put_line('The email of '|| emps(idx).first_name 
                ||' '||emps(idx).last_name||' is : '|| emps(idx).email);
            idx := emps.prior(idx);
        end loop;
        end;
        ---------------An example of inserting with associative arrays
        create table employees_salary_history as select * from employees where 1=2;
        alter table employees_salary_history add insert_date date;
        select * from employees_salary_history;
        /
        declare
        type e_list is table of employees_salary_history%rowtype index by pls_integer;
        emps e_list;
        idx pls_integer;
        begin
            for x in 100 .. 110 loop
            select e.*,'01-JUN-20' into emps(x) from employees e
            where employee_id = x;
        end loop;
        idx := emps.first;
        while idx is not null loop
            emps(idx).salary := emps(idx).salary + emps(idx).salary*0.2;
            insert into employees_salary_history values emps(idx);
            dbms_output.put_line('The employee '|| emps(idx).first_name 
                                ||' is inserted to the history table');
            idx := emps.next(idx);
        end loop;
        end;
        /
        drop table employees_salary_history;
        ```
    - In memory tables. 

- Storing collections in tables:
    - You can store VArrays and Nested tables in a table column to avoid an inner join (better performance).
    - Oracle doesnt store records directly in the database (only temporal in an anonymous block). Instead of records we use object. 
    - VArrays can be in the same table or in an automatically generated table.
    - With nested tables in a column they are stored in a different table every time. This table is called "storage table", as it is in the same segment of the actual table the joins are very fast. The inserts in this table are done by the system. 
    - You cannot drop to modify a type if it is table dependent (its used in a table column).
    ```SQL
    ---------------Storing Varray Example
    create or replace type t_phone_number as object (p_type varchar2(10), p_number varchar2(50));
    /
    create or replace type v_phone_numbers as varray(3) of t_phone_number;
    /
    create table emps_with_phones (employee_id number,
                                first_name varchar2(50),
                                last_name varchar2(50),
                                phone_number v_phone_numbers);
    /
    select * from emps_with_phones;
    /
    insert into emps_with_phones values (10,'Alex','Brown',v_phone_numbers(
                                                                    t_phone_number('HOME','111.111.1111'),
                                                                    t_phone_number('WORK','222.222.2222'),
                                                                    t_phone_number('MOBILE','333.333.3333')
                                                                    ));
    insert into emps_with_phones values (11,'Bob','Green',v_phone_numbers(
                                                                    t_phone_number('HOME','000.000.000'),
                                                                    t_phone_number('WORK','444.444.4444')
                                                                    ));                                                                
    /
    ---------------Querying the varray example
    select e.first_name,last_name,p.p_type,p.p_number from emps_with_phones e, table(e.phone_number) p;
    ---------------The codes for the storing nested table example
    create or replace type n_phone_numbers as table of t_phone_number;
    /
    create table emps_with_phones2 (employee_id number,
                                first_name varchar2(50),
                                last_name varchar2(50),
                                phone_number n_phone_numbers)
                                NESTED TABLE phone_number STORE AS phone_numbers_table;
    /
    select * from emps_with_phones2;
    /
    insert into emps_with_phones2 values (10,'Alex','Brown',n_phone_numbers(
                                                                    t_phone_number('HOME','111.111.1111'),
                                                                    t_phone_number('WORK','222.222.2222'),
                                                                    t_phone_number('MOBILE','333.333.3333')
                                                                    ));
    insert into emps_with_phones2 values (11,'Bob','Green',n_phone_numbers(
                                                                    t_phone_number('HOME','000.000.000'),
                                                                    t_phone_number('WORK','444.444.4444')
                                                                    ));      
    /
    select e.first_name,last_name,p.p_type,p.p_number from emps_with_phones2 e, table(e.phone_number) p;
    ---------------new insert and update
    insert into emps_with_phones2 values (11,'Bob','Green',n_phone_numbers(
                                                                    t_phone_number('HOME','000.000.000'),
                                                                    t_phone_number('WORK','444.444.4444'),
                                                                    t_phone_number('WORK2','444.444.4444'),
                                                                    t_phone_number('WORK3','444.444.4444'),
                                                                    t_phone_number('WORK4','444.444.4444'),
                                                                    t_phone_number('WORK5','444.444.4444')
                                                                    ));    
    select * from emps_with_phones2;
    update emps_with_phones2 set phone_number = n_phone_numbers(
                                                                    t_phone_number('HOME','000.000.000'),
                                                                    t_phone_number('WORK','444.444.4444'),
                                                                    t_phone_number('WORK2','444.444.4444'),
                                                                    t_phone_number('WORK3','444.444.4444'),
                                                                    t_phone_number('WORK4','444.444.4444'),
                                                                    t_phone_number('WORK5','444.444.4444')
                                                                    )
    where employee_id = 11;
    ---------------Adding a new value into the nested table inside of a table
    declare
    p_num n_phone_numbers;
    begin
    select phone_number into p_num from emps_with_phones2 where employee_id = 10;
    p_num.extend;
    p_num(5) := t_phone_number('FAX','999.99.9999');
    UPDATE emps_with_phones2 set phone_number = p_num where employee_id = 10;
    end;
    ```
- **Sql cursors:**

    - Cursors are pointers to the data.
    - Implicit cursors (to handle the selects) and explicit cursors. 
    - Cursors more efficient than collections.
    - You cannot go back in cursors, only in collections.
    - Cursor:
        - Declare (allocate memory in the db).
        - Open
        - Fetch (the rows from the active set of rows). 
        - Check (if we finish iterating).
        - Close.
    
    declare
        cursor cursor_name is select_statement;
    begin
        open cursor_name;
        fetch cursor_name into variables, records etc;
        close cursor_name;
    end; 


    ```SQL
    declare
    cursor c_emps is select first_name,last_name from employees;
    v_first_name employees.first_name%type;
    v_last_name employees.last_name%type;
    begin
        open c_emps;
        fetch c_emps into v_first_name,v_last_name;
        fetch c_emps into v_first_name,v_last_name;
        fetch c_emps into v_first_name,v_last_name;
        dbms_output.put_line(v_first_name|| ' ' || v_last_name);
        fetch c_emps into v_first_name,v_last_name;
        dbms_output.put_line(v_first_name|| ' ' || v_last_name);
        close c_emps;
    end;
    --------------- cursor with join example
    declare
    cursor c_emps is select first_name,last_name, department_name from employees
                        join departments using (department_id)
                        where department_id between 30 and 60;
    v_first_name employees.first_name%type;
    v_last_name employees.last_name%type;
    v_department_name departments.department_name%type;
    begin
    open c_emps;
    fetch c_emps into v_first_name, v_last_name,v_department_name;
    dbms_output.put_line(v_first_name|| ' ' || v_last_name|| ' in the department of '|| v_department_name);
    close c_emps;
    end;
    ```

    -  If we have to retrieve many columns in the select statement we use records instead of variables.
    -  The simplest must useful way to work with cursors is with a record of the type of the cursor. This way we get a variable with the same number of columns retrieved by the cursor easily.
    ```SQL
    declare
    type r_emp is record (  v_first_name employees.first_name%type,
                            v_last_name employees.last_name%type);
    v_emp r_emp;
    cursor c_emps is select first_name,last_name from employees;
    begin
        open c_emps;
        fetch c_emps into v_emp;
        dbms_output.put_line(v_emp.v_first_name|| ' ' || v_emp.v_last_name);
        close c_emps;
    end;
    --------------- An example for using cursors table rowtype
    declare
    v_emp employees%rowtype;
    cursor c_emps is select first_name,last_name from employees;
    begin
        open c_emps;
        fetch c_emps into v_emp.first_name,v_emp.last_name;
        dbms_output.put_line(v_emp.first_name|| ' ' || v_emp.last_name);
        close c_emps;
    end;
    --------------- An example for using cursors with cursor%rowtype.
    declare
    cursor c_emps is select first_name,last_name from employees;
    v_emp c_emps%rowtype;
    begin
        open c_emps;
        fetch c_emps into v_emp.first_name,v_emp.last_name;
        dbms_output.put_line(v_emp.first_name|| ' ' || v_emp.last_name);
        close c_emps;
    end;
    ```
    - Looping with cursors:
    ```SQL
    declare
    cursor c_emps is select * from employees where department_id = 30;
    v_emps c_emps%rowtype;
    begin
    open c_emps;
    loop
        fetch c_emps into v_emps;
        dbms_output.put_line(v_emps.employee_id|| ' ' ||v_emps.first_name|| ' ' ||v_emps.last_name);
    end loop;
    close c_emps;
    end; 
    ---------------%notfound example
    declare
    cursor c_emps is select * from employees where department_id = 30;
    v_emps c_emps%rowtype;
    begin
    open c_emps;
    loop
        fetch c_emps into v_emps;
        exit when c_emps%notfound;
        dbms_output.put_line(v_emps.employee_id|| ' ' ||v_emps.first_name|| ' ' ||v_emps.last_name);
    end loop;
    close c_emps;
    end;
    ---------------while loop example
    declare
    cursor c_emps is select * from employees where department_id = 30;
    v_emps c_emps%rowtype;
    begin
    open c_emps;
    fetch c_emps into v_emps;
    while c_emps%found loop
        dbms_output.put_line(v_emps.employee_id|| ' ' ||v_emps.first_name|| ' ' ||v_emps.last_name);
        fetch c_emps into v_emps;
        --exit when c_emps%notfound;
    end loop;
    close c_emps;
    end;
    ---------------for loop with cursor example
    declare
    cursor c_emps is select * from employees where department_id = 30;
    v_emps c_emps%rowtype;
    begin
    open c_emps;
    for i in 1..6 loop
        fetch c_emps into v_emps;
        dbms_output.put_line(v_emps.employee_id|| ' ' ||v_emps.first_name|| ' ' ||v_emps.last_name);
    end loop;
    close c_emps;
    end;
    ----- ----------FOR..IN clause example
    declare
    cursor c_emps is select * from employees where department_id = 30;
    begin
    for i in c_emps loop
        dbms_output.put_line(i.employee_id|| ' ' ||i.first_name|| ' ' ||i.last_name);
    end loop;
    end;
    ---------------FOR..IN with select example
    begin
    for i in (select * from employees where department_id = 30) loop
        dbms_output.put_line(i.employee_id|| ' ' ||i.first_name|| ' ' ||i.last_name);
    end loop;
    end;
    ```
    - **Cursors with parameters:**
        declare 
            cursor cursor_name (parameter_name datatype...,)
            is select_statement
        begin
            open cursor_name(parameter_values);
            fetch cursor_name into variables, records, etc;
            close cursor_name;
        end; 
        - We don't specify the precision in the parameter's datatype.
        - Bind parameters with :b_parameter_name.
        ```SQL
        declare
        cursor c_emps (p_dept_id number) is select first_name,last_name,department_name 
                            from employees join departments using (department_id)
                            where department_id = p_dept_id;
        v_emps c_emps%rowtype;
        begin
        open c_emps(20);
        fetch c_emps into v_emps;
            dbms_output.put_line('The employees in department of '|| v_emps.department_name|| ' are :');
            close c_emps;
        open c_emps(20);
            loop
            fetch c_emps into v_emps;
            exit when c_emps%notfound;
            dbms_output.put_line(v_emps.first_name|| ' ' ||v_emps.last_name);
            end loop;
        close c_emps;
        end;
        --------------- bind variables as parameters
        declare
        cursor c_emps (p_dept_id number) is select first_name,last_name,department_name 
                            from employees join departments using (department_id)
                            where department_id = p_dept_id;
        v_emps c_emps%rowtype;
        begin
        open c_emps(:b_emp);
        fetch c_emps into v_emps;
            dbms_output.put_line('The employees in department of '|| v_emps.department_name|| ' are :');
            close c_emps;
        open c_emps(:b_emp);
            loop
            fetch c_emps into v_emps;
            exit when c_emps%notfound;
            dbms_output.put_line(v_emps.first_name|| ' ' ||v_emps.last_name);
            end loop;
        close c_emps;
        end;
        ---------------cursors with two different parameters
        declare
        cursor c_emps (p_dept_id number) is select first_name,last_name,department_name 
                            from employees join departments using (department_id)
                            where department_id = p_dept_id;
        v_emps c_emps%rowtype;
        begin
        open c_emps(:b_dept_id);
        fetch c_emps into v_emps;
            dbms_output.put_line('The employees in department of '|| v_emps.department_name|| ' are :');
            close c_emps;
        open c_emps(:b_dept_id);
            loop
            fetch c_emps into v_emps;
            exit when c_emps%notfound;
            dbms_output.put_line(v_emps.first_name|| ' ' ||v_emps.last_name);
            end loop;
        close c_emps;
        
        open c_emps(:b_dept_id2);
        fetch c_emps into v_emps;
            dbms_output.put_line('The employees in department of '|| v_emps.department_name|| ' are :');
            close c_emps;
        open c_emps(:b_dept_id2);
            loop
            fetch c_emps into v_emps;
            exit when c_emps%notfound;
            dbms_output.put_line(v_emps.first_name|| ' ' ||v_emps.last_name);
            end loop;
        close c_emps;
        end;
        --------------- cursor with parameters - for in loops
        declare
        cursor c_emps (p_dept_id number) is select first_name,last_name,department_name 
                            from employees join departments using (department_id)
                            where department_id = p_dept_id;
        v_emps c_emps%rowtype;
        begin
        open c_emps(:b_dept_id);
        fetch c_emps into v_emps;
            dbms_output.put_line('The employees in department of '|| v_emps.department_name|| ' are :');
            close c_emps;
        open c_emps(:b_dept_id);
            loop
            fetch c_emps into v_emps;
            exit when c_emps%notfound;
            dbms_output.put_line(v_emps.first_name|| ' ' ||v_emps.last_name);
            end loop;
        close c_emps;
        
        open c_emps(:b_dept_id2);
        fetch c_emps into v_emps;
            dbms_output.put_line('The employees in department of '|| v_emps.department_name|| ' are :');
            close c_emps;
            
            for i in c_emps(:b_dept_id2) loop
            dbms_output.put_line(i.first_name|| ' ' ||i.last_name);
            end loop;
        end;
        ---------------cursors with multiple parameters
        declare
        cursor c_emps (p_dept_id number , p_job_id varchar2) is select first_name,last_name,job_id,department_name 
                            from employees join departments using (department_id)
                            where department_id = p_dept_id
                            and job_id = p_job_id;
        v_emps c_emps%rowtype;
        begin
            for i in c_emps(50,'ST_MAN') loop
            dbms_output.put_line(i.first_name|| ' ' ||i.last_name|| ' - ' || i.job_id);
            end loop;
            dbms_output.put_line(' - ');
            for i in c_emps(80,'SA_MAN') loop
            dbms_output.put_line(i.first_name|| ' ' ||i.last_name|| ' - ' || i.job_id);
            end loop;
        end;
        --------------- An error example of using parameter name with the column name
        declare
        cursor c_emps (p_dept_id number , job_id varchar2) is select first_name,last_name,job_id,department_name 
                            from employees join departments using (department_id)
                            where department_id = p_dept_id
                            and job_id = job_id;
        v_emps c_emps%rowtype;
        begin
            for i in c_emps(50,'ST_MAN') loop
            dbms_output.put_line(i.first_name|| ' ' ||i.last_name|| ' - ' || i.job_id);
            end loop;
            dbms_output.put_line(' - ');
            for i in c_emps(80,'SA_MAN') loop
            dbms_output.put_line(i.first_name|| ' ' ||i.last_name|| ' - ' || i.job_id);
            end loop;
        end;
        ```
    - **PL SQL Cursor attributes:**
        - %FOUND
        - %NOTFOUND
        - %ISOPEN
        - %ROWCOUNT: returns the number of rows fetched (error if not open).
        ```SQL
        declare
        cursor c_emps is select * from employees where department_id = 50;
        v_emps c_emps%rowtype;
        begin
        if not c_emps%isopen then
            open c_emps;
            dbms_output.put_line('hello');
        end if;
        dbms_output.put_line(c_emps%rowcount);
        fetch c_emps into v_emps;
        dbms_output.put_line(c_emps%rowcount);
        dbms_output.put_line(c_emps%rowcount);
        fetch c_emps into v_emps;
        dbms_output.put_line(c_emps%rowcount);
        close c_emps;
        
        open c_emps;
            loop
            fetch c_emps into v_emps;
            exit when c_emps%notfound or c_emps%rowcount>5;
            dbms_output.put_line(c_emps%rowcount|| ' ' ||v_emps.first_name|| ' ' ||v_emps.last_name);
            end loop;
        close c_emps;
        end;
        ``` 
    - **For update clause:**
        - Locks the rows of the select and keep them locked until you close the cursor.
        - nowait option will terminate the execution if there is a lock.
        - Default option is wait (specify number of seconds to wait and then exit if rows are still locked).
        - for update of to lock only the specified columns.

        cursor cursor_name (parameter_name datatype, ...)
            is select_statement
            for update (of colums) [nowait | wait n]
        
        ```SQL
        grant create session to my_user;
        grant select any table to my_user;
        grant update on hr.employees_copy to my_user;
        grant update on hr.departments to my_user;
        UPDATE EMPLOYEES_COPY SET PHONE_NUMBER = '1' WHERE EMPLOYEE_ID = 100;
        declare
        cursor c_emps is select employee_id,first_name,last_name,department_name
            from employees_copy join departments using (department_id)
            where employee_id in (100,101,102)
            for update;
        begin
        /* for r_emps in c_emps loop
            update employees_copy set phone_number = 3
            where employee_id = r_emps.employee_id; 
        end loop; */
        open c_emps;
        end;
        --------------- example of wait with second
        declare
        cursor c_emps is select employee_id,first_name,last_name,department_name
            from employees_copy join departments using (department_id)
            where employee_id in (100,101,102)
            for update of employees_copy.phone_number, 
            departments.location_id wait 5;
        begin
        /* for r_emps in c_emps loop
            update employees_copy set phone_number = 3
            where employee_id = r_emps.employee_id; 
        end loop; */
        open c_emps;
        end;
        ---------------example of nowait
        declare
        cursor c_emps is select employee_id,first_name,last_name,department_name
            from employees_copy join departments using (department_id)
            where employee_id in (100,101,102)
            for update of employees_copy.phone_number, 
            departments.location_id nowait;
        begin
        /* for r_emps in c_emps loop
            update employees_copy set phone_number = 3
            where employee_id = r_emps.employee_id; 
        end loop; */
        open c_emps;
        end;
        ```
    - **Where current of clause:**
        - Used together with the for update clause.
        - Faster than updating using the PK.
            - First get the related rows from the index.
            - Then gets the row id from the column.
            - Then does the update.

        where current of cursor_name.
        - It uses the row id so it is fast.
        - We cannot use it with joins, group functions, etc. (because it doesn't have a rowid in the result set).
        ```SQL
        declare
        cursor c_emps is select * from employees 
                    where department_id = 30 for update;
        begin
        for r_emps in c_emps loop
            update employees set salary = salary + 60
                where current of c_emps;
        end loop;  
        end;
        ---------------Wrong example of using where current of clause
        declare
        cursor c_emps is select e.* from employees e, departments d
                            where 
                            e.department_id = d.department_id
                            and e.department_id = 30 for update;
        begin
        for r_emps in c_emps loop
            update employees set salary = salary + 60
                where current of c_emps;
        end loop;  
        end;
        ---------------An example of using rowid like where current of clause
        declare
        cursor c_emps is select e.rowid,e.salary from employees e, departments d
                            where 
                            e.department_id = d.department_id
                            and e.department_id = 30 for update;
        begin
        for r_emps in c_emps loop
            update employees set salary = salary + 60
                where rowid = r_emps.rowid;
        end loop;  
        end;
        ```
    - **Ref cursors:**
        - Pointers (in memory) to the actual cursors.
        - We cannot assign null values.
        - Use in table-views create codes.
        - Store in collections.
        - Compare.
        - Specify a return type and dynamicacly create the query for the ref cursor.
        - There are two types: restricted and unrestricted. 

        type cursor_type_name is ref cursor [return type]
        open cursor_variable_name for query;

        - If we use a table type as a return type we use %rowtype.
        - If we use a record type that has a table's type we use %type.
        - If we have manually created a record type and use it as a return type we simply write the name of the record type.
        - To create a dynamic query for a cursor it has to be a weak type cursor. 
        - We cannot create a record of the type of the weak cursor.
        - Weak cursors can have bind variable. Assing value to the variable with the using keyword.
        - Oracle predifined weak cursor: **sys_refcursor** (instead of creating a type).
        ```SQL
        declare
        type t_emps is ref cursor return employees%rowtype;
        rc_emps t_emps;
        r_emps employees%rowtype;
        begin
        open rc_emps for select * from employees;
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name);
            end loop;
        close rc_emps;
        end;
        --------------- in two different queries
        declare
        type t_emps is ref cursor return employees%rowtype;
        rc_emps t_emps;
        r_emps employees%rowtype;
        begin
        open rc_emps for select * from retired_employees;
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name);
            end loop;
        close rc_emps;
        
        dbms_output.put_line('--------------');
        
        open rc_emps for select * from employees where job_id = 'IT_PROG';
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name);
            end loop;
        close rc_emps;
        end;
        ---------------Example of using with %type when declaring records first
        declare
        r_emps employees%rowtype;
        type t_emps is ref cursor return r_emps%type;
        rc_emps t_emps;
        --type t_emps2 is ref cursor return rc_emps%rowtype;
        begin
        open rc_emps for select * from retired_employees;
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name);
            end loop;
        close rc_emps;
        
        dbms_output.put_line('--------------');
        
        open rc_emps for select * from employees where job_id = 'IT_PROG';
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name);
            end loop;
        close rc_emps;
        end;
        ---------------manually declared record type with cursors example
        declare
        type ty_emps is record (e_id number, 
                                first_name employees.last_name%type, 
                                last_name employees.last_name%type,
                                department_name departments.department_name%type);
        r_emps ty_emps;
        type t_emps is ref cursor return ty_emps;
        rc_emps t_emps;
        begin
        open rc_emps for select employee_id,first_name,last_name,department_name 
                            from employees join departments using (department_id);
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name|| 
                    ' is at the department of : '|| r_emps.department_name );
            end loop;
        close rc_emps;
        end;
        ---------------first example of weak ref cursors
        declare
        type ty_emps is record (e_id number, 
                                first_name employees.last_name%type, 
                                last_name employees.last_name%type,
                                department_name departments.department_name%type);
        r_emps ty_emps;
        type t_emps is ref cursor;
        rc_emps t_emps;
        q varchar2(200);
        begin
        q := 'select employee_id,first_name,last_name,department_name 
                            from employees join departments using (department_id)';
        open rc_emps for q;
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name|| 
                    ' is at the department of : '|| r_emps.department_name );
            end loop;
        close rc_emps;
        end;
        --------------- bind variables with cursors example
        declare
        type ty_emps is record (e_id number, 
                                first_name employees.last_name%type, 
                                last_name employees.last_name%type,
                                department_name departments.department_name%type);
        r_emps ty_emps;
        type t_emps is ref cursor;
        rc_emps t_emps;
        r_depts departments%rowtype;
        --r t_emps%rowtype;
        q varchar2(200);
        begin
        q := 'select employee_id,first_name,last_name,department_name 
                            from employees join departments using (department_id)
                            where department_id = :t';
        open rc_emps for q using '50';
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name|| 
                    ' is at the department of : '|| r_emps.department_name );
            end loop;
        close rc_emps;
        
        open rc_emps for select * from departments;
            loop
            fetch rc_emps into r_depts;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_depts.department_id|| ' ' || r_depts.department_name);
            end loop;
        close rc_emps;
        end;
        ---------------sys_refcursor example
        declare
        type ty_emps is record (e_id number, 
                                first_name employees.last_name%type, 
                                last_name employees.last_name%type,
                                department_name departments.department_name%type);
        r_emps ty_emps;
        -- type t_emps is ref cursor;
        rc_emps sys_refcursor;
        r_depts departments%rowtype;
        --r t_emps%rowtype;
        q varchar2(200);
        begin
        q := 'select employee_id,first_name,last_name,department_name 
                            from employees join departments using (department_id)
                            where department_id = :t';
        open rc_emps for q using '50';
            loop
            fetch rc_emps into r_emps;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_emps.first_name|| ' ' || r_emps.last_name|| 
                    ' is at the department of : '|| r_emps.department_name );
            end loop;
        close rc_emps;
        
        open rc_emps for select * from departments;
            loop
            fetch rc_emps into r_depts;
            exit when rc_emps%notfound;
            dbms_output.put_line(r_depts.department_id|| ' ' || r_depts.department_name);
            end loop;
        close rc_emps;
        end;
        ```
- **Exceptions:**
    - Runtime errors may damage your data.
    - We can explicit raise an exception.
    - We can handle exceptions in 3 ways: 
        1. Trap (write a method to handle it).
        2. Propagate (default option)
        3. Trap and propagate. 
    - 3 types of exceptions:
        1. Predefined oracle server errors.
        2. Nonpredefined oracle server errors.
        3. User-defined errors.
    - If we want to deal with the non predefined exceptions we need to declare them in the declaration section.
    - sqlcode returns the code of the exception.
    - sqlerrm returns the sql error message.
    - Inner blocks can have their own exceptions.
    ```SQL
    ----------------- Handling the exception
    declare
    v_name varchar2(6);
    begin
    select first_name into v_name from employees where employee_id = 50;
    dbms_output.put_line('Hello');
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the selected id');
    end;
    ----------------- Handling multiple exceptions
    declare
    v_name varchar2(6);
    v_department_name varchar2(100);
    begin
    select first_name into v_name from employees where employee_id = 100;
    select department_id into v_department_name from employees where first_name = v_name;
    dbms_output.put_line('Hello '|| v_name || '. Your department id is : '|| v_department_name );
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the selected id');
    when too_many_rows then
        dbms_output.put_line('There are more than one employees with the name '|| v_name);
        dbms_output.put_line('Try with a different employee');
    end;
    ----------------- when others then example
    declare
    v_name varchar2(6);
    v_department_name varchar2(100);
    begin
    select first_name into v_name from employees where employee_id = 103;
    select department_id into v_department_name from employees where first_name = v_name;
    dbms_output.put_line('Hello '|| v_name || '. Your department id is : '|| v_department_name );
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the selected id');
    when too_many_rows then
        dbms_output.put_line('There are more than one employees with the name '|| v_name);
        dbms_output.put_line('Try with a different employee');
    when others then
        dbms_output.put_line('An unexpected error happened. Connect with the programmer..');
    end;
    ----------------- sqlerrm & sqlcode example
    declare
    v_name varchar2(6);
    v_department_name varchar2(100);
    begin
    select first_name into v_name from employees where employee_id = 103;
    select department_id into v_department_name from employees where first_name = v_name;
    dbms_output.put_line('Hello '|| v_name || '. Your department id is : '|| v_department_name );
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the selected id');
    when too_many_rows then
        dbms_output.put_line('There are more than one employees with the name '|| v_name);
        dbms_output.put_line('Try with a different employee');
    when others then
        dbms_output.put_line('An unexpected error happened. Connect with the programmer..');
        dbms_output.put_line(sqlcode || ' ---> '|| sqlerrm);
    end;
    ----------------- Inner block exception example
    declare
    v_name varchar2(6);
    v_department_name varchar2(100);
    begin
    select first_name into v_name from employees where employee_id = 100;
    begin
        select department_id into v_department_name from employees where first_name = v_name;
        exception
        when too_many_rows then
        v_department_name := 'Error in department_name';
    end;
    dbms_output.put_line('Hello '|| v_name || '. Your department id is : '|| v_department_name );
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the selected id');
    when too_many_rows then
        dbms_output.put_line('There are more than one employees with the name '|| v_name);
        dbms_output.put_line('Try with a different employee');
    when others then
        dbms_output.put_line('An unexpected error happened. Connect with the programmer..');
        dbms_output.put_line(sqlcode || ' ---> '|| sqlerrm);
    end;
    /
    select * from employees where first_name = 'Steven';
    ```
    - Non predefined exceptions (unnamed exceptions).
    - We cannot trap with the error codes.
    - We declare exceptions with the eror codes.
    
    exception_name EXCEPTION;
    PRAGMA EXCEPTION_INIT(exception_name, error_code)

    - Pragma is a compiler directive, to provide an instruction to the compiler.
    ```SQL
    begin
    UPDATE employees_copy set email = null where employee_id = 100;
    end;
    -----------------HANDLING a nonpredefined exception
    declare
    cannot_update_to_null exception;
    pragma exception_init(cannot_update_to_null,-01407);
    begin
    UPDATE employees_copy set email = null where employee_id = 100;
    exception
    when cannot_update_to_null then
        dbms_output.put_line('You cannot update with a null value!');
    end;
    ```
    - In user-defined exception we dont use an Oracle error number because we don't have one.
    - We can raise our exception before or after handling it.
    ```SQL
    ----------------- creating a user defined exception
    declare
    too_high_salary exception;
    v_salary_check pls_integer;
    begin
    select salary into v_salary_check from employees where employee_id = 100;
    if v_salary_check > 20000 then
        raise too_high_salary;
    end if;
    --we do our business if the salary is under 2000
    dbms_output.put_line('The salary is in an acceptable range');
    exception
    when too_high_salary then
    dbms_output.put_line('This salary is too high. You need to decrease it.');
    end;
    ----------------- raising a predefined exception
    declare
    too_high_salary exception;
    v_salary_check pls_integer;
    begin
    select salary into v_salary_check from employees where employee_id = 100;
    if v_salary_check > 20000 then
        raise invalid_number;
    end if;
    --we do our business if the salary is under 2000
    dbms_output.put_line('The salary is in an acceptable range');
    exception
    when invalid_number then
        dbms_output.put_line('This salary is too high. You need to decrease it.');
    end;
    ----------------- raising inside of the exception
    declare
    too_high_salary exception;
    v_salary_check pls_integer;
    begin
    select salary into v_salary_check from employees where employee_id = 100;
    if v_salary_check > 20000 then
        raise invalid_number;
    end if;
    --we do our business if the salary is under 2000
    dbms_output.put_line('The salary is in an acceptable range');
    exception
    when invalid_number then
        dbms_output.put_line('This salary is too high. You need to decrease it.');
    raise;
    end;
    ```
    - Raise_application_error to raise applications out of the block.
    - Raises the error to the caller (for business exceptions).
    
    raise_application_error(error_number, error_message [, TRUE | FALSE])

    - Third parameter is the error stack. If false all the other error messages are deleted.
    - Will stop execution (need to be handled with WHEN OTHERS).
    - Error between -20000 and 20999.
    ```SQL
    declare
    too_high_salary exception;
    v_salary_check pls_integer;
    begin
    select salary into v_salary_check from employees where employee_id = 100;
    if v_salary_check > 20000 then
        --raise too_high_salary;
    raise_application_error(-20243,'The salary of the selected employee is too high!');
    end if;
    --we do our business if the salary is under 2000
    dbms_output.put_line('The salary is in an acceptable range');
    exception
    when too_high_salary then
    dbms_output.put_line('This salary is too high. You need to decrease it.');
    end;
    ----------------- raise inside of the exception section
    declare
    too_high_salary exception;
    v_salary_check pls_integer;
    begin
    select salary into v_salary_check from employees where employee_id = 100;
    if v_salary_check > 20000 then
        raise too_high_salary;
    end if;
    --we do our business if the salary is under 2000
    dbms_output.put_line('The salary is in an acceptable range');
    exception
    when too_high_salary then
    dbms_output.put_line('This salary is too high. You need to decrease it.');
    raise_application_error(-01403,'The salary of the selected employee is too high!',true);
    end;
    ```
- **Functions and stored procedures:**
    - Stored procedures:

    CREATE [OR REPLACE] PROCEDURE procedure_name
    [(parameter_name) [IN | OUT | IN OUT] type [,...]]{IS | AS}
    BEGIN
    ...
    EXCEPTION
    ...
    END;
    - Two ways to execute a procedure: inside an anonymous block or with the Execute keyword.
    ```SQL
    ----------------- Creating a procedure
    create procedure increase_salaries as
        cursor c_emps is select * from employees_copy for update;
        v_salary_increase number := 1.10;
        v_old_salary number;
    begin
        for r_emp in c_emps loop
        v_old_salary := r_emp.salary;
        r_emp.salary := r_emp.salary * v_salary_increase + r_emp.salary * nvl(r_emp.commission_pct,0);
        update employees_copy set row = r_emp where current of c_emps;
        dbms_output.put_line('The salary of : '|| r_emp.employee_id 
                                || ' is increased from '||v_old_salary||' to '||r_emp.salary);
        end loop;
    end;
    ----------------- Multiple procedure usage
    begin
    dbms_output.put_line('Increasing the salaries!...');
    INCREASE_SALARIES;
    INCREASE_SALARIES;
    INCREASE_SALARIES;
    INCREASE_SALARIES;
    dbms_output.put_line('All the salaries are successfully increased!...');
    end;
    ----------------- Different procedures in one block
    begin
    dbms_output.put_line('Increasing the salaries!...');
    INCREASE_SALARIES;
    new_line;
    INCREASE_SALARIES;
    new_line;
    INCREASE_SALARIES;
    new_line;
    INCREASE_SALARIES;
    dbms_output.put_line('All the salaries are successfully increased!...');
    end;
    -----------------Creating a procedure to ease the dbms_output.put_line procedure 
    create procedure new_line as
    begin
    dbms_output.put_line('------------------------------------------');
    end;
    -----------------Modifying the procedure with using the OR REPLACE command.
    create or replace procedure increase_salaries as
        cursor c_emps is select * from employees_copy for update;
        v_salary_increase number := 1.10;
        v_old_salary number;
    begin
        for r_emp in c_emps loop
        v_old_salary := r_emp.salary;
        r_emp.salary := r_emp.salary * v_salary_increase + r_emp.salary * nvl(r_emp.commission_pct,0);
        update employees_copy set row = r_emp where current of c_emps;
        dbms_output.put_line('The salary of : '|| r_emp.employee_id 
                                || ' is increased from '||v_old_salary||' to '||r_emp.salary);
        end loop;
        dbms_output.put_line('Procedure finished executing!');
    end
    ```
    - For IN parameters you can write the IN keyword or leave it empty and it will be IN parameter by default.
    - When we use IN OUT at the same time we receive a parameter and return the same parameter and return it after the procedure finish.
    - We need variables to get the out parameters.
    ```SQL
    -----------------Creating a procedure with the IN parameters
    create or replace procedure increase_salaries (v_salary_increase in number, v_department_id pls_integer) as
        cursor c_emps is select * from employees_copy where department_id = v_department_id for update;
        v_old_salary number;
    begin
        for r_emp in c_emps loop
        v_old_salary := r_emp.salary;
        r_emp.salary := r_emp.salary * v_salary_increase + r_emp.salary * nvl(r_emp.commission_pct,0);
        update employees_copy set row = r_emp where current of c_emps;
        dbms_output.put_line('The salary of : '|| r_emp.employee_id 
                                || ' is increased from '||v_old_salary||' to '||r_emp.salary);
        end loop;
        dbms_output.put_line('Procedure finished executing!');
    end;
    ----------------- Creating a procedure with the OUT parameters
    create or replace procedure increase_salaries 
        (v_salary_increase in out number, v_department_id pls_integer, v_affected_employee_count out number) as
        cursor c_emps is select * from employees_copy where department_id = v_department_id for update;
        v_old_salary number;
        v_sal_inc number := 0;
    begin
        v_affected_employee_count := 0;
        for r_emp in c_emps loop
        v_old_salary := r_emp.salary;
        r_emp.salary := r_emp.salary * v_salary_increase + r_emp.salary * nvl(r_emp.commission_pct,0);
        update employees_copy set row = r_emp where current of c_emps;
        dbms_output.put_line('The salary of : '|| r_emp.employee_id 
                                || ' is increased from '||v_old_salary||' to '||r_emp.salary);
        v_affected_employee_count := v_affected_employee_count + 1;
        v_sal_inc := v_sal_inc + v_salary_increase + nvl(r_emp.commission_pct,0);
        end loop;
        v_salary_increase := v_sal_inc / v_affected_employee_count;
        dbms_output.put_line('Procedure finished executing!');
    end;
    -----------------Another example of creating a procedure with the IN parameter 
    CREATE OR REPLACE PROCEDURE PRINT(TEXT IN VARCHAR2) IS
    BEGIN
    DBMS_OUTPUT.PUT_LINE(TEXT);
    END;
    -----------------Using the procedures that has the IN parameters 
    begin
    PRINT('SALARY INCREASE STARTED!..');
    INCREASE_SALARIES(1.15,90);
    PRINT('SALARY INCREASE FINISHED!..');
    end;
    -----------------Using the procedure that has OUT parameters 
    declare
    v_sal_inc number := 1.2;
    v_aff_emp_count number;
    begin
    PRINT('SALARY INCREASE STARTED!..');
    INCREASE_SALARIES(v_sal_inc,80,v_aff_emp_count);
    PRINT('The affected employee count is : '|| v_aff_emp_count);
    PRINT('The average salary increase is : '|| v_sal_inc || ' percent!..');
    PRINT('SALARY INCREASE FINISHED!..');
    end;
    ```
    - We can only use default parameters for IN parameters. For out and in out it raises a compilation error.
    
    CREATE [OR REPLACE] PROCEDURE procedure_name
    [(parameter_name) [IN | OUT | IN OUT] type DEFAULT value|expression [,...]]{IS | AS}
    BEGIN
    ...
    EXCEPTION
    ...
    END;

    - Don't pass null directly to the default parameters.
    - Named notations allows us to pass parameters independent from the position.
    
    EXECUTE PROCEDURE_NAME(parameter_name => value | expression)

     - When it has both positional and named notations at the same time it is called mixed notation. But you cannot use positional notation AFTER named notation.
    - You can use named notation for both in and out parameters.
    ```SQL
        ----------------- A standard procedure creation with a default value
    create or replace PROCEDURE PRINT(TEXT IN VARCHAR2 := 'This is the print function!.') IS
    BEGIN
    DBMS_OUTPUT.PUT_LINE(TEXT);
    END;
    -----------------Executing a procedure without any parameter. It runs because it has a default value.
    exec print();
    -----------------Running a procedure with null value will not use the default value 
    exec print(null);
    -----------------Procedure creation of a default value usage
    create or replace procedure add_job(job_id pls_integer, job_title varchar2, 
                                        min_salary number default 1000, max_salary number default null) is
    begin
    insert into jobs values (job_id,job_title,min_salary,max_salary);
    print('The job : '|| job_title || ' is inserted!..');
    end;
    -----------------A standard run of the procedure
    exec ADD_JOB('IT_DIR','IT Director',5000,20000); 
    -----------------Running a procedure with using the default values
    exec ADD_JOB('IT_DIR2','IT Director',5000); 
    -----------------Running a procedure with the named notation
    exec ADD_JOB('IT_DIR5','IT Director',max_salary=>10000); 
    -----------------Running a procedure with the named notation example 2
    exec ADD_JOB(job_title=>'IT Director',job_id=>'IT_DIR7',max_salary=>10000 , min_salary=>500);
    ``` 
    - Functions must return some value.
    - Functions can also get IN and OUT parameters.
    - You cannot run functions standalone and can be used within a select statement.

    CREATE [OR REPLACE] FUNCTION function_name [(parameter_name [IN | OUT | IN OUT] type DEFAULT value|expression [, ...])] RETURN return_data_type {IS | AS}

    - We can return null value even if we have a return type.
    - To use a function in SQL expression they must return a valid SQL Data type (not a record for example). Also cannot call a function that contains a DML Statement. 
    - You need to have execute privilege to execute a function.
    - If you click run in a function it display a pop-up window with an anonymous block to test your function.

    - To drop a function:

    DROP FUNCTION function_name;

    ```SQL
    CREATE OR REPLACE FUNCTION get_avg_sal (p_dept_id departments.department_id%type) RETURN number AS 
    v_avg_sal number;
    BEGIN
    select avg(salary) into v_avg_sal from employees where department_id = p_dept_id;
    RETURN v_avg_sal;
    END get_avg_sal;
    ----------------- using a function in begin-end block
    declare
    v_avg_salary number;
    begin
    v_avg_salary := get_avg_sal(50);
    dbms_output.put_line(v_avg_salary);
    end;
    ----------------- using functions in a select clause
    select employee_id,first_name,salary,department_id,get_avg_sal(department_id) avg_sal from employees;
    ----------------- using functions in group by, order by, where clauses 
    select get_avg_sal(department_id) from employees
    where salary > get_avg_sal(department_id)
    group by get_avg_sal(department_id) 
    order by get_avg_sal(department_id);
    ----------------- dropping a function
    drop function get_avg_sal;
    ```
    - Local subprogram: we can create subprograms inside of an anonymous blocks or in another subprogram.
    - Local functions or procedure have to be at THE END OF the Declaration section.
    ```SQL
        ----------------- creating and using subprograms in anonymous blocks - false usage
    create table emps_high_paid as select * from employees where 1=2;
    /
    declare
    procedure insert_high_paid_emp(emp_id employees.employee_id%type) is 
        emp employees%rowtype;
        begin
        emp := get_emp(emp_id);
        insert into emps_high_paid values emp;
        end;
    function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
        emp employees%rowtype;
        begin
        select * into emp from employees where employee_id = emp_num;
        return emp;
        end;
    begin
    ```
    - You can overload local subprograms and package subprograms but not standalone subprograms.
    - If parameters are in the same family or subtype it won't work.
    ```SQL
    declare
    procedure insert_high_paid_emp(p_emp employees%rowtype) is 
        emp employees%rowtype;
        e_id number;
        function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
        begin
            select * into emp from employees where employee_id = emp_num;
            return emp;
        end;
        function get_emp(emp_email employees.email%type) return employees%rowtype is
        begin
            select * into emp from employees where email = emp_email;
            return emp;
        end;
        begin
        emp := get_emp(p_emp.employee_id);
        insert into emps_high_paid values emp;
        end;
    begin
    for r_emp in (select * from employees) loop
        if r_emp.salary > 15000 then
        insert_high_paid_emp(r_emp);
        end if;
    end loop;
    end;
    ----------------- overloading with multiple usages
    declare
    procedure insert_high_paid_emp(p_emp employees%rowtype) is 
        emp employees%rowtype;
        e_id number;
        function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
        begin
            select * into emp from employees where employee_id = emp_num;
            return emp;
        end;
        function get_emp(emp_email employees.email%type) return employees%rowtype is
        begin
            select * into emp from employees where email = emp_email;
            return emp;
        end;
        function get_emp(f_name varchar2, l_name varchar2) return employees%rowtype is
        begin
            select * into emp from employees where first_name = f_name and last_name = l_name;
            return emp;
        end;
        begin
        emp := get_emp(p_emp.employee_id);
        insert into emps_high_paid values emp;
        emp := get_emp(p_emp.email);
        insert into emps_high_paid values emp;
        emp := get_emp(p_emp.first_name,p_emp.last_name);
        insert into emps_high_paid values emp;
        end;
    begin
    for r_emp in (select * from employees) loop
        if r_emp.salary > 15000 then
        insert_high_paid_emp(r_emp);
        end if;
    end loop;
    end;
    ```
    - You should handle the exception in the subprograms as well as in the main programs that call them.

    ```SQL
    ----------------- An unhandled exception in function
    create or replace function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
    emp employees%rowtype;
    begin
    select * into emp from employees where employee_id = emp_num;
    return emp;
    end;
    ----------------- calling that function in an anonymous block
    declare
    v_emp employees%rowtype;
    begin
    dbms_output.put_line('Fetching the employee data!..');
    v_emp := get_emp(10);
    dbms_output.put_line('Some information of the employee are : ');
    dbms_output.put_line('The name of the employee is : '|| v_emp.first_name);
    dbms_output.put_line('The email of the employee is : '|| v_emp.email);
    dbms_output.put_line('The salary of the employee is : '|| v_emp.salary);
    end;
    ----------------- hanling the exception wihout the return clause - not working
    create or replace function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
    emp employees%rowtype;
    begin
    select * into emp from employees where employee_id = emp_num;
    return emp;
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the id' || emp_num);
    end;
    /*handling and raising the exception*/
    
    create or replace function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
    emp employees%rowtype;
    begin
    select * into emp from employees where employee_id = emp_num;
    return emp;
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the id '|| emp_num);
        raise no_data_found;
    end;
    ----------------- handling all possible exception cases
    create or replace function get_emp(emp_num employees.employee_id%type) return employees%rowtype is
    emp employees%rowtype;
    begin
    select * into emp from employees where employee_id = emp_num;
    return emp;
    exception
    when no_data_found then
        dbms_output.put_line('There is no employee with the id '|| emp_num);
        raise no_data_found;
    when others then
        dbms_output.put_line('Something unexpected happened!.');
    return null;
    end;
    ```
    - You can filter in the functions right clicking and select Apply filter. You have to clear the filter after you use. It remains even if you close the sql developer.
    - We can also search for procedures and other objects in the User_source, dba_source and all_source data dictionary views.
        - They have name (procedure_name for example), type (procedure), line (line number) and text (line's content).
    - If you drop the function or procedure you will also remove the privileges of that function. If you create or replace the function/procedure the privileges remain.
- **Regular table functions:**
    - Table functions can be queried as a regular table.
    - They return a table of varrays or nested tables.
    - Regular table function create the data table in memory and then returns the whole thing.
- **Pipelined functions:**
    - They return each row as soon as it has it.
    - You will not have memory problems.
- If your collection returns a small amount of data use a regular, otherwise use pipelined.
- In order to call them as a table you need to use the table operator (for older versions of oracle).
- When you apply order by clause to pipelined it is slow but still faster than regular table functions.
    ```SQL
    ----------------- Creating the type
    CREATE TYPE t_day AS OBJECT (
    v_date DATE,
    v_day_number INT
    );
    ----------------- creating a nested table type
    create type t_days_tab is table of t_day;
    ----------------- creating a regular table function
    create or replace function f_get_days(p_start_date date , p_day_number int) 
                return t_days_tab is
    v_days t_days_tab := t_days_tab();
    begin
    for i in 1 .. p_day_number loop
    v_days.extend();
    v_days(i).v_date := p_start_date + i;
    v_days(i).v_day_number := to_number(to_char(v_days(i).v_date,'DDD'));
    end loop;
    return v_days;
    end;
    ----------------- querying from the regular table function
    select * from table(f_get_days(sysdate,1000000));
    ----------------- querying from the regular table function without the table operator
    select * from f_get_days(sysdate,1000000);
    ----------------- creating a pipelined table function
    create or replace function f_get_days_piped (p_start_date date , p_day_number int) 
                return t_days_tab PIPELINED is
    begin
    for i in 1 .. p_day_number loop
    PIPE ROW (t_day(p_start_date + i,
                    to_number(to_char(p_start_date + i,'DDD'))));
    end loop;
    RETURN;
    end;
    ----------------- querying from the pipelined table function
    select * from f_get_days_piped(sysdate,1000000)
    ```

- **Packages:**
    - They group subprograms, types, variables, etc in one container.
    - PGA: Program Global Area. When you call a fuction or procedure it takes from the storage and moves it to the PGA (in the database memory).
    - When a user calls a function or proceduce from a package, the whole package is loaded in the SGA, so it doesnt have to load it again when another user calls something from the same package.
    - SGA: System global area (shared by all users in the dba).
    - Memory only stores the information, it is executed by the processor.
    - Variables exists only in the PGA, when a user calls a variable from a package it takes it from the SGA and moves it to the PGA.
    - Modularity.
    - Easy maintenance.
    - Encapsulation and security: only the functions specified in the package specs are accesible (the ones specified in the body dont).
    - Performance.
    - Functionality (persistence of viable and cursors in packages).
    - Overloading.
    - The packages consists of two parts:
        - Package specification (specs). F
            - For globalization, declaration part.
            - This objects are public.
        - Package body. 
            - Implementation of the functions declared in the specs.
            - If a subprogram is not specified in the spec is not visible to the others (private).
    
    CREATE OR REPLACE PACKAGE Package_name AS
    END Package_name;
    
    - When we create a procedure inside a package we don't use Create or Replace keywords, only PROCEDURE (in the declaration)
    - If the body is not created and it shows in the specs it raises a runtime error.
    
    CREATE OR REPLACE PACKAGE BODY Package_name AS
    END Package_name;

    - Use short and meaningful names for the packages.
    - Good practice to compile both the spec and the body when you make changes.
    - You cannot rename a package, when you do that it creates a new one. You have to create a new one and drop the old one.

    DROP PACKAGE package_name; (drops both spec and body).

    DROP PACKAGE BODY package_name; (only the body).

    - You can create a bodyless package to provide global variables.
    ```SQL
    --------------------------------------------------------------------------------------------------------------------
    --------------------------------------------CREATING & USING PACKAGES-----------------------------------------------
    --------------------------------------------------------------------------------------------------------------------
    ----------------- Creating first package specification
    CREATE OR REPLACE 
    PACKAGE EMP AS 
    v_salary_increase_rate number := 0.057; 
    cursor cur_emps is select * from employees;
    
    procedure increase_salaries;
    function get_avg_sal(p_dept_id int) return number;
    END EMP;
    ----------------- Creating the package body
    CREATE OR REPLACE
    PACKAGE BODY EMP AS
    procedure increase_salaries AS
    BEGIN
        for r1 in cur_emps loop
        update employees_copy set salary = salary + salary * v_salary_increase_rate;
        end loop;
    END increase_salaries;
    function get_avg_sal(p_dept_id int) return number AS
    v_avg_sal number := 0;
    BEGIN
        select avg(salary) into v_avg_sal from employees_copy where
            department_id = p_dept_id;
        RETURN v_avg_sal;
    END get_avg_sal;
    END EMP;
    ----------------- using the subprograms in packages
    exec EMP_PKG.increase_salaries;
    ----------------- using the variables in packages
    begin
    dbms_output.put_line(emp_pkg.get_avg_sal(50));
    dbms_output.put_line(emp_pkg.v_salary_increase_rate);
    end;
    ```
    - Local variables inside the package body must be declared at the beginning of the code block. Right after the IS/AS keywords.
    - Public (in the spec), private (only in the body), local (inside code block in the body, unreachable even for other body objects).
    ```SQL
    ---------------------------------------------------------------------------------------------
    --------------------------------VISIBILITY OF VARIABLES IN PACKAGES--------------------------
    ---------------------------------------------------------------------------------------------
    create or replace PACKAGE BODY EMP_PKG AS
    
    v_sal_inc int := 500;
    v_sal_inc2 int := 500;
    
    procedure print_test is
    begin
        dbms_output.put_line('Test : '|| v_sal_inc);
    end;
    
    procedure increase_salaries AS
    BEGIN
        for r1 in cur_emps loop
        update employees_copy set salary = salary + salary * v_salary_increase_rate
        where employee_id = r1.employee_id;
        end loop;
    END increase_salaries;
    function get_avg_sal(p_dept_id int) return number AS
    v_avg_sal number := 0;
    BEGIN
        print_test;
        select avg(salary) into v_avg_sal from employees_copy where
            department_id = p_dept_id;
        RETURN v_avg_sal;
    END get_avg_sal;
    
    END EMP_PKG;
    ----------------- 
    create or replace PACKAGE BODY EMP_PKG AS
    
    v_sal_inc int := 500;
    v_sal_inc2 int := 500;
    function get_sal(e_id employees.employee_id%type) return number;
    procedure print_test is
    begin
        dbms_output.put_line('Test : '|| v_sal_inc);
        dbms_output.put_line('Test salary : '|| get_sal(102));
    end;
    procedure increase_salaries AS
    BEGIN
        for r1 in cur_emps loop
        update employees_copy set salary = salary + salary * v_salary_increase_rate
        where employee_id = r1.employee_id;
        end loop;
    END increase_salaries;
    function get_avg_sal(p_dept_id int) return number AS
    v_avg_sal number := 0;
    BEGIN
        print_test;
        select avg(salary) into v_avg_sal from employees_copy where
            department_id = p_dept_id;
        RETURN v_avg_sal;
    END get_avg_sal;
    
    function get_sal(e_id employees.employee_id%type) return number is
    v_sal number := 0;
    begin
        select salary into v_sal from employees where employee_id = e_id;
    end;
    
    end;
    ```
    - You cannot call a sub program before you create it (forward reference).
    - Oracle is a block structured program.
    - Forward declaration: define the subprogram signature in the variable's declaration section and implement it below.
    - Initialization block inside the body of the package. Declare a Begin keyword right before the end of the package and it will work as a constructor of the package.
    - It only runs the first time the package is called in a session.
    