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
- 
