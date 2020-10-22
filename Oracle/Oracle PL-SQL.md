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
