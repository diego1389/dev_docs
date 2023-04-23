## CS 01

- **.sln:** a solution file. Solutions have folders and folders have files.
- **outer folder:** is the solution folder. Inside the main folder are the project folders.
- **bin folder:** .dll file is the .net asembly created when we ran our application in vs.

## CS 07

- **Main types:** String, bool, int and double. 
[Ver más](https://msdn.microsoft.com/en-us/library/cs7y5x0x(v=vs.90).aspx)
- datatype nameOfTheVariable;

- .net framework types, common set of types behind the scenes.

## CS 09

- i += 1;
Increment operator: i++;
Decrement operator: i--;

- Beware of down casting:
    ```cs
    double myDouble = 5.5;
    int myInt = 7;
    int myResult = (int)myDouble + myInt;//12

    int mySecondInt = 7;
    int myOtherInt = 4;
    double myOtherResult = (double)mySecondInt / (double)myOtherInt;//1.75
    ```
- You can check for arithmetic operations:
    ```cs
    checked{
        /*arithmetic operation with potential of overflow.*/
    }
    ```

## CS 010

- **Expression:** sequence operators (variable names, controls, literals, objects) and operands (entity on which an operator acts), working together. 

- **Statements:** action that a program takes.

## CS 011

- **==** for equality.

```cs
if(){
	
}else if(){
	
}else{
	
}
```

- For radio buttons use same group name to keep them related.

## CS 012

- **Ternary operator:** something.Text = (boolean expression) ? result if true : result if false;

    ```cs
    resultLabel.Text = (firstValueTextBox.Text == secondValueTextBox.Text) ? "Yes!They're equal." : "No! They're not equal.";
    ```
- **Comparison:** == <= >= != 
    ```cs
    if(!testCheckBox.Checked){}
    ```
- **Logical:** && means *and* / || means *or*

- **&&** have precedence over the **||**, it can be fixed with parenthesis.

## CS 014

```cs
DateTime myValue = DateTime.Now;
resultLabel.Text = myValue.ToString();
```

- Many ways to *format* time string: toLongDateString, toLocalTime, toShortDateString, etc.

- To add days:
```cs
DateTime myValue = DateTime.Now;
resultLabel.Text = myValue.AddDays(2).ToString(); /*Chaining methods with . */
```
- To substract 2 days:

```cs
resultLabel.Text = myValue.AddMonths(-2).ToString();
resultLabel.Text = myValue.Month.ToString();
resultLabel.Text = myValue.DayOfWeek.ToString(); 
```
- Initialize Datetime in a different time than now:
```cs
DateTime myValue = new DateTime(1989, 06, 30);
/*With minutes and seconds: */
DateTime myValue = new DateTime(1989, 06, 30, 7, 15, 20);
```

## CS 015

- Ctrl + url: opens url inside VS.

- Timespan object represents the amount of time between two datetimes.

- TimeSpan receives a string formated as: 
Days.Hours:Minutes:Seconds.Milliseconds
```cs
TimeSpan myTimeSpan = TimeSpan.Parse("1.2:3:30.5"); /* 1 day, 2 hours, 3 minutes, etc..*/
```
- Getting a timespan substracting a datetime:

```cs
DateTime myBirthDay = new DateTime(1989, 6, 30);

TimeSpan myAge = DateTime.Now.Subtract(myBirthDay);
```
- To get the total amount of days from a timespan:

```cs
resultLabel.Text = myAge.TotalDays.ToString();
```

- You can customize all the styles.

- Also you can change the NameDayFormat.

- **Smart tag:** auto format to choose a prebuild theme.

- Getdate from calendar:
    ```cs
    resultLabel.Text = myCalendar.SelectedDate.ToShortDateString();
    ```
- Set date in the calendar (doesn't "go" to that date):
    ```cs
     myCalendar.SelectedDate = DateTime.Parse("20/02/2017");
    ```
- Show or go to the decided date use:
    ```cs
    myCalendar.VisibleDate = DateTime.Parse("12/02/1989");    
    ```
- Change the selection mode programaticalle to select weeks:
    ```cs
    resultLabel.Text = "The week of: " + myCalendar.SelectedDate.ToShortDateString();
    ```

## CS 017

- Page_Load = Initialize the values of the server controls.
    ```cs
    Datetime.Now.Date.AddDays(2); /*Truncates Datetime into a date to initialize a Calendar control.*/
    ```
- **IsPostBack** = Not the first time it loads:
    ```cs
    If(!Page.IsPostBack){}
    ```
## CS 018 (Debugging)

- **Locals window:** all the variables and objects in the line of code.

- **Watch window:** watch a specific variable no matter where i am.

- **Debugging:** red means the value changed in the previous line of code.

- Smart tag of server control during debugging (little arrow hovering over).

- You can pin variable out as reminder of last value.

- You can change variable's value during debuggin g (it turns red).

- You can drag and drop a control into the watch window and watch all its properties.

- Inmediate code = Execute code "externally" to change values or properties of the the control in the watch window and watch the changes.

- When you stop debuggin the little pin show you the last value it had.

## CS 019

- Template String.Format
    ```cs
    string result = String.Format("Thank you, {0}, for your business. Your SSN is: {1}", nameTextBox.Text, ssnTextBox.Text); 
    ```
- String formating only works with integers. For example, to format SSN:

    ```cs
    string result = String.Format("Thank you, {0}, for your business. Your SSN is: {1:000-00-0000}", nameTextBox.Text, ssn); /*Thank you, Diego, for your business. Your SSN is: 304-38-0402*/
    
    /*If not enough numbers adds zeros at the beginning*/
    ```
- Check custom and format date string:
    ```cs
    string result = String.Format("<br/> Loan Date: {0:ddd -- d, M, yy}",loanCalendar.SelectedDate); //Loan Date: sáb. -- 14, 7, 18
    ```
- Currency:
    ```cs
    string result = String.Format("<br/> Your salary: {0:C}", salary); //Your salary: $650,000.00
    ```

- To prevent the page to go to the top on Postback go to Web.Config and add:

    ```xml
    <pages maintainScrollPositionOnPostBack ="true"/>
    ```

## CS 020

- **WWW:** stateless environment. Whenever you send request to the web server it preserves nothing.

- **Viewstate:** to preserve values between roundtrips to the web server.

- If you add a server control textbox and a an html input tag, they look the same but when you send the request to the web server the text you typed in the textbox remains but in the input tag disappears.

    ```html
        <div class="aspNetHidden">
        <input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="9Mxg65fjoktVMLN7OdARHg/+HVvZvjqqTAd4HJzyBrR3w3wRR6ald+y1qiDTzD0CO9jRyE1kQbJtSPUim/mjZKT0PAGkqUPlHJTddQpZj2g=" />
        </div>
    ```
- Somewhere in the value there is the word test we typed in the server control. When you use a lot of asp controls this input value grows a lot and slows the page.

- Pass value using ViewState:
    ```cs
    protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState.Add("myValue", "Diego");
            }
        }

        protected void okButton_Click(object sender, EventArgs e)
        {
            string value = ViewState["myValue"].ToString();
            resultLabel.Text = value;
        }
    ```
- Store values in the viewState:

    ```cs
    protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState.Add("myValue", "");
            }
        }

        protected void okButton_Click(object sender, EventArgs e)
        {
            string value = ViewState["myValue"].ToString();
            value += valueTextBox.Text + " ";
            ViewState["myValue"] = value;
            resultLabel.Text = value;
            valueTextBox.Text = string.Empty;
        }
    ```
## CS 021 (Mantaining State with View Sate)

- Array, initialize it:
    ```cs
    string[] values = new string[5] {"Good", "bye", "say", "hello", "hello"};  
    ```

- Add value:
    ```cs
    values[2] = value3TextBox.Text;
	resultLabel.Text = values[2];
    ```
- Add to viewstate:
    ```cs
    ViewState.Add("myValues", values);  
    ```
- Retrieve from viewstate. (Cast into array):
    ```cs
     string[] values = (string[])ViewState["myValues"];
    ```

##CS 022

- Declare multidimensional array:
    ```cs
    double[,] priceGrid = new double[3, 3];
    ```
- Fill it:
    ```cs
    priceGrid[0, 1] = 350.00;
    ```
- Retrieve element:
    ```cs
    resultLabel.Text = priceGrid[fromCity, toCity].ToString();
    ```
## CS 023

- How to resize an existing array (which is "naturally" inmmutable)
    ```cs
    Array.Resize(ref hours, hours.Length + 1); /*creates new bucket in the computer's memory with the new size.*/

	protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            double[] workingHours = new double[0];
            ViewState.Add("Hours", workingHours);
        }
    }

    protected void okButton_Click(object sender,EventArgs e)
    {
        double[] hours = (double[])ViewState["Hours"];
        Array.Resize(ref hours, hours.Length + 1);
        int newestItem = hours.GetUpperBound(0);/*retrieves highest index*/
        hours[newestItem] = double.Parse(hoursTextBox.Text);

        ViewState["Hours"] = hours;
        resultLabel.Text = string.Format("Total hours: {0} <br /> Most hours: {1} <br /> Least hours: {2}<br /> Average  hours: {3:N2}<br />", hours.Sum(),
                hours.Max(),
                hours.Min(),
                hours.Average());/*Sum() performs the sum of a sequence of double values*/
    }
    ```
## CS 024 (variable scope)

- You can declare a variable outside a code block and use it inside a code block but not the other way around.

# CS 025

- The deeper you nest the more confusing your code will become.
    ```cs
    if (firstCheckBox.Checked)
            {
                if (secondCheckBox.Checked)
                {
                    if (thirdCheckBox.Checked)
                    {
                        resultLabel.Text = "They are all checked!";
                    }
                }
            }
    ```
- To avoid messy nested ifs you can use:
    ```cs
    if (!firstCheckBox.Checked) return;
    if (!secondCheckBox.Checked) return;
    if (!thirdCheckBox.Checked) return;
    resultLabel.Text = "They are all checked!";
    ```

- You can create a for loop template by typing For + Tab + Tab.
- If you change the word i for index or any other word and type tab it will replace (refactor) all the i variables. 
- It has a description of snipet.
- To step for loop by 3 
    ```cs
    for (int j = 5; j <= 15; j+=3)
    {
        result += "<br/>" + j.ToString();
    }
    ```
- Order an array:
    ```cs
    Array.Sort(names);   
    ```
## CS 026

- To reverse the results:
    ```cs
    Array.Reverse(names);
    ```
 - To reverse them sorted:
    ```cs
    Array.Sort(names);
    Array.Reverse(names);
    ``` 
- **break** keyword.

## CS 027

- while + tab + tab: while loop snipet.
- To get a random number between 1 and 10:
    ```cs
    Random random = new Random();
 	int heroDamage = random.Next(1, 10);
    ```
 - To concatenate and add 1 to a variable
    ```cs
    result += "<br/>Round " + ++round;
    ```
## CS 028 (Defining and calling helper methods )

- A method is a block of code with a name. Use cases: 
    1. When you need the same block of code in different places.
    2. When there is a complex problem that can be break into small little problems. 
- Your code should read like a story.
- **Autopostback:** post from the client to the server.
- **Trim:** trim of empty spaces from string.

- To check if it's a double:
    ```cs
    double quantity = 0.0;
	if (!Double.TryParse(quantityTextBox.Text, out quantity)) /*returns true or false if double and also a second parameter in the out part*/
    return;
    ```
- Get all the repetitive code in the radio checked event and add a method:  
    ```cs
    private void calculateCups()
    {
        if (quantityTextBox.Text.Trim().Length == 0)
            return;

        double quantity = 0.0;
        if (!Double.TryParse(quantityTextBox.Text, out quantity))
            return;

            double cups = 0.0;
            if (fromCupsRadio.Checked) cups = quantity;
            else if (fromPintsRadio.Checked) cups = quantity * 2;
            else if (fromQuartsRadio.Checked) cups = quantity * 4;
            else if (fromGallonsRadio.Checked) cups = quantity * 16;

            resultLabel.Text = "The number of cups: " + cups.ToString();

        }
    ```
- **DRY:** Don't Repeat Yourself
		
## CS 029 (Creating Methods with input parameters)

    ```cs
    private void calculateCups(double measureToCupRatio)
    {
        if (quantityTextBox.Text.Trim().Length == 0)
            return;

        double quantity = 0.0;
        if (!Double.TryParse(quantityTextBox.Text, out quantity))
            return;

        double cups = 0.0;

        cups = quantity * measureToCupRatio;
        resultLabel.Text = "The number of cups: " + cups.ToString();
    }
    ```
## CS 030

- Keep the helper method in the order that they are used inside the main body of code.
-  Single responsability principle: one method can have one and only one responsability. A good method should be around 6 - 10 lines of code. A method should have only one reason to exist.
- Helper methods can call helper methods to solve small problems.
    ```cs
    private int performAttack(int defenderHealth, int attackerDamageMax, string attackerName, string defenderName)
    {
        Random random = new Random();
        int damage = random.Next(1, attackerDamageMax);
        defenderHealth -= damage;

        describeRound(attackerName, defenderName, defenderHealth);
        return defenderHealth;
    }

    private void describeRound(string attackerName, string defenderName, int defenderHealth)
    {
        if (defenderHealth <= 0)
            resultLabel.Text += String.Format("<br />{0} attacks {1} and vanquishes the {2}.", attackerName, defenderName, defenderName);
        else
            resultLabel.Text += String.Format("<br />{0} attacks {1}, leaving {2} with {3} health.", attackerName, defenderName, defenderName, defenderHealth);
    }
    ```

- By default the Random class uses the current datetime. In the while loop it occurs super fast so it creates almost the same number because it uses the same seed. To avoid that behavior you have to place the instance of random outside the loop.

# CS 031 (Overloaded methods )

- For things that perform the same sort of job.

- All the overloaded methods must have a different signature (different number of parameters or different daa types)

- .NET framework sees it as three different methods.

- If it has the same number of parameters and the same datatype it will produce an error, even though you intend to use the parameters for something different.

- In IntelliSense you can see the description of the next overloaded methods with the down arrow in the keyboard.
 
## CS 032 (Optional parameters)

- Optional parameters have default values. If you do not provide a default value it won't work. Optional paramters must be at the end of the parameter's list.
    ```cs
    private int performAttack(int defenderHealth, 
            int attackerDamageMax, 
            string attackerName, 
            string defenderName,
            double criticalHitChance = .1)
    {
            Random random = new Random();
            int damage = random.Next(1, attackerDamageMax);
            defenderHealth -= damage;

            describeRound(attackerName, defenderName, defenderHealth);
            return defenderHealth;
    }
    ```
## CS 033 (named parameters)

- You can skip optional parameters naming them:
    ```cs
    private int performAttack(int defenderHealth, 
            int attackerDamageMax, 
            string attackerName, 
            string defenderName, 
            double criticalHitChance = .1, 
            double defenderArmorBonus = 5)
    {
        Random random = new Random();
        int damage = random.Next(1, attackerDamageMax);
        defenderHealth -= damage;

        describeRound(attackerName, defenderName, defenderHealth);
        return defenderHealth;
    }

	heroHealth = performAttack(heroHealth, 20, "Monster", "Hero", defenderArmorBonus : 3); 
    ```

## CS 034 (Creating methods with Output parameters)

- To output a value from a parameter you have to declare it like this:
    ```cs
    private bool defeatEnemy(int defenderHealth, 
            int attackerDamageMax, 
            string attackerName, 
            string defenderName,
            out int remainingDefenderHealth)
    {
        Random random = new Random();
        int damage = random.Next(1, attackerDamageMax);
        remainingDefenderHealth = defenderHealth - damage;

        if (remainingDefenderHealth <= 0) return true;
        else return false;
    }
    ```
- Now you can get the boolean value and also the remainingDefenderHealth. Not recommended because of the single responsability principle.

## CS 035 (Manipulating strings)

- Escape character:
    ```cs
    resultLabel.Text = "<p style=\"color:#ee3b32\"> Hi</p>";
    ```
- To get the third element of a string (not the best way):
    ```cs
    string value = stringTextBox.Text;
    resultLabel.Text = value[2].ToString();
    ```
- **Helper methods:**
    1. StartsWith:
        ```cs
        resultLabel.Text = (value.StartsWith("A")) ?  "The value starts with A" : "The values does not start with A";
        ```
    2. EndsWith:
        ```cs
        resultLabel.Text = (value.EndsWith(".")) ?  "The value ends with ." : "The values does not end with .";
        ```
    3. Contains: 
        ```cs
        resultLabel.Text = (value.Contains("good")) ?  "The value contains 'good'" : "The values does not contain 'good'";
        ```
    4. IndexOf:
        ```cs
        int index = value.IndexOf("good");
        resultLabel.Text = "'Good' begins at index " + index;
        ```
    5. Insert:
        ```cs
        int index = value.IndexOf("good");
        resultLabel.Text = value.Insert(index, "*screams*");
        ```
	6. Remove:
        ```cs
        resultLabel.Text = value.Remove(index, value.Length - index);
        ```
    7. Substring:
        ```cs
        int index = value.IndexOf("good");
        resultLabel.Text = value.Substring(index, 4);
        ```
    8. Trim, TrimStart, TrimEnd:
        ```cs
        resultLabel.Text = string.Format("The length before trim is {0} <br/>The length after trim is {1}", value.Length, value.Trim().Length);
        ```
    9. PadLeft, PadRight (to fill a particular string with characters to be an specific number of characters):
        ```cs
        resultLabel.Text = value.PadLeft(10, '*');
        ```
    10. ToUpper, ToLower

    11. Replace:
        ```cs
        result.Text = value.Replace("something", "something else");
        ```
    12. Split:
        ```cs
        string result = "";
        string[] values = stringTextBox.Text.Split(',');
        for (int i = 0; i < values.Length; i++)
        {
            result += values[i] + " " + values[i].Length + "<br/>";
        }
        resultLabel.Text = result;
        ```
    13. **String builder:** class to append many strings together. More memory efficient way but use it when you have to concatenate lots of lines:
        ```cs
        StringBuilder sb = new StringBuilder();
        string[] values = stringTextBox.Text.Split(',');
        for (int i = 0; i < values.Length; i++)
        {
            sb.Append(values[i]);
            sb.Append(" ");
            sb.Append(values[i].Length);
            sb.Append("<br/>");
        }
        resultLabel.Text = sb.ToString();
        ```

## CS 036 (Classes and objects)

- Everything in C# and .NET framework either is a class of a part of it.
- Properties (attributes of the class):
    ```cs
    //You can create them with prop + TAB + TAB
     public string Make { get; set; }
     public int Year { get; set; }
    ```
- Object is an instance of a class

- **new** keyword is like a factory:
    ```cs
    Car myNewCar = new Car();
    ```
- Every object inherits from System.Object, that is why they have some methods like toString()

- Set properties:
	```cs
    myNewCar.Make = "OldsMobile";
    ``` 
- Get properties:
    ```cs
    resultLabel.Text = string.Format("{0} - {1} - {2} - {3}", myNewCar.Make, myNewCar.Model, myNewCar.Year, myNewCar.Color);
    ```
- Pass objects to methods:
    ```cs
    private double determineMarketValue(Car car)
    {
            double carValue = 0.0;

            if (car.Year == 1986) carValue = 100.0;
            return carValue;
    }
    Car myNewCar = new Car();
    myNewCar.Make = "OldsMobile";
    myNewCar.Model = "Cutlas Supreme";
    myNewCar.Year = 1986;
    myNewCar.Color = "Silver";

    double carMarketValue = determineMarketValue(myNewCar);

    resultLabel.Text = string.Format("{0} - {1} - {2} - {3}", myNewCar.Make, myNewCar.Model, myNewCar.Year,  carMarketValue);
    ```
- To use a method inside a class:
    ```cs
    class Car
    {
        //Properties
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        //Methods
        public double DetermineMarketValue()
        {
            double carValue = 0.0;

            if (this.Year == 1986) carValue = 100.0;
            return carValue;
        }
    }
    ```
- To call it from another class:
    ```cs
    Car myNewCar = new Car();
    myNewCar.Make = "OldsMobile";
    myNewCar.Model = "Cutlas Supreme";
    myNewCar.Year = 1986;
    myNewCar.Color = "Silver";

    double carMarketValue = myNewCar.DetermineMarketValue();

    resultLabel.Text = string.Format("{0} - {1} - {2} - {3} - {4:C}", myNewCar.Make, myNewCar.Model, myNewCar.Year, myNewCar.Color, carMarketValue);
    ```
## CS 037 (Creating class files, Creating cohesive classes and code navigation)

- **Cohesion:** how related the members of the class are. Single responsability to creating classes. 

- Classes should have single responsability in the system.  

- Single responsability also says that a method or class should have only one reason to change.

- Names that are small and compact.

- Go to the definition -> right click -> find all references.

## CS 038 (Understanding Object Reference and Object Lifetime)

- When you create an instance of an object the .Net framework has to create a spot in the computer's memory large enough to hold a new instance of the Car class. The memory has addresses and there is where the .Net framework temporary stores values during the lifetime of the object. 
    ```cs
    Car myCar; 
	myCar = new Car(); /*holds a refernce of the address in the computer's memory*/
    ```
- Garbarge collection counts how many references are there to objects in memory.
    ```cs
    Car myCar = new Car();
	Car myOtherCar = myCar;/*to monitors holding addresses to a bucket in the computer's memory for that instance of Car class.*/
    ```
- You lose the instance of an object? (not longer holding a reference) when:
    1. You try  to access it out of scope (out of certain block of code).
    2. You set the instance to null:
        ```cs
        myCar = null;
	    myOtherCar = null;
        ```
    3. GarbageCollection (specially when the object is holding a refence to a system resource like a network connection or a file in the file system or a handle to access a database).

## CS 039 (Understanding the .NET Framework)

- Two parts of .Net that concerns us:
    1. The class library. 
        1. A library of code that Microsoft wrote to take care of difficult codes. (Math operators, string builders, etc, transmitting information over the internet, etc).
        2. **FCL:** (Framework class library)
        3. **API:** Application Programming Interface
        4.  All the dlls that we import are located in C:\Windows\Microsoft.NET\Framework\v4.0.30319
    2. The runtime (Command Language Runtime). 
        1. Protective bubble that wraps the application. 
        2. It takes care of the low-level details (how to interact  with the OS). 
        3. It provides a layer of protection for the end user, mostly for windows application. 
        4. The runtime is the application runtime machine (similar to Java Virtual Machine). 
        5. The compiler creates .NET assemblies which are filled with Microsoft Intermediate Language or MSIL and stores it in a .ddl (dynamic linked library) or .exe file. It bridges the gap between the code that you write and the Web Server.
        6. The application is compiled twice: the first time that you run the application or the assembly on a new machine the .Net framework runtime performs a second compilation on that MSIL version (this second version will create an optimized version of your assembly for that particular configuration, hardware and software of the computer)

- Intermediate Language Disassembler, to see what was generated
C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\ildasm.exe

- Dissasembles so you can write C# or VB code again in the application

- Obfuscator: to protect your application from dissasemblers.

- All the .Net programming languages compile down to the same intermediate language (MSIL).

## CS 040 (Namespaces and the using directive)

- How to access code that is not in your project by default:
All the classes are organized in namespaces. You can think of a namespace like a last name for your classes. It is possible that a class name is used many times in different areas. 

- You can reference a class by its full name.
    ```cs
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //System is a huge namespace, Text is a "sub" namespace
    ```

- The compiler will ignore the namespaces that are not needed.

- When you need a using directive it will appear a blue little dash so you can click Ctrl + . to navigate through the options and to choose the namespace. 

- You can change the Default namespace of your project by changing its properties.

-  When you have the same class name from 2 different class libraries you can change the using to be like:
    ```cs
    Using Administration = ACME.Accounting.Administration;
    ```
- And call it normally
    ```cs
    Administration a = new Administration();
    ```
## CS 041 (Creating class libraries)

- Add new project to the solution -> Windows Root -> Class library template -> Add the classes inside the template (overwrite the class1, they have to be public) and build.

- Then, in the project you have to create a reference to the assembly

- Add using HeroMonster.

- Right click in the target project -> Add reference -> Projects -> Name of the class library 
Now you can instanciate the class from the class library: 
    ```cs
    Character c = new Character();
    ```
- When you include the classes in the class library you partitioned the business logic and the domain object.

- Ctrl + H = Find and replace.

## CS 042 (Accesibility modifiers)

- Accesibility modifiers:
    1. **Internal:** is the default for classes. The class can be accessed by any element in the *SAME* assembly (project, class library).
    2. **Private:** it can be accessed only inside of the same class. By default methods are private.
    3. **Protected:** can only be accessed by code in the same class or in a class that is derived from that class.

- **Encapsulation:** the internal implementation of an object is generally hidden from view outside the object's definition. It prevents users from setting the internal data of the component. 
- **Coupling objects:** two object that do not know much of each other. (not good)

- You have to hide the internal elements of your system from the final user. Example of television, you dont know how it works "behind".

- *Private fields:* you want to keep an object or a variable around for the private use of your class implementation or as a backing for a public variable.

- To create a more complex property = propfull + Tab + Tab (you can add complexity to your property declaration):
    ```cs
    public int MyProperty
    {
        get { return myVar; }
        set {
            if (value < 100)
            {
                myVar = value;
            } else throw new Exception();
               
        }
    }
    ```
- **propg + Tab + Tab:** it creates a property with public get and private set. 
    ```cs
    public int MyProperty { get; private set; }
    ```
- You define a data type (a class) and the properties of that class. You can add properties that are complex data types.

## CS 043 (Constructor)

- The way that you can set the state of new object at the point of instantiation is through a special method called constructor. 
- It is executed at the moment that a new class is created as an object in the computer's memory and set properties to a default or to some value that you allow the user to pass in to the method. Constructor's name must be the same as the class name.
- If we don't write the constructor it is added automatically (empty and does nothing).
    ```cs
    Car myCar = new Car(); /*(); because it is a method invocation (the constructor method)*/

    public Car(){ }//overwrites the default constructor

    public class Car
    {
        public string Make{ get; set; }


        public Car()
        {
            this.Make = "Undefined";
        }

        public string FormatDetailsForDisplay()
        {
            return String.Format("Make {0}", Make);
        }
    }
    ```
- Code snippet to create constructor: ctor + Tab + Tab

- Overload constructor:
    ```cs
    public class Car
    {
        public string Make{ get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        public Car()
        {
            this.Make = "Undefined";
            this.Model = "Undefined";
            this.Year = 1980;
            this.Color = "Undefined";
        }

        public Car(string make, string model, int year, string color)
        {
            this.Make = make;
            this.Model = model;
            this.Year = year;
            this.Color = color;
        }

        public string FormatDetailsForDisplay()
        {
            return String.Format("Make {0} - Model {1} - Year {2} - Color {3}", Make, Model,Year, Color);
        }
    }

    Car myCar = new Car("Oldmobile", "Cutlas Supreme", 1985, "Silver");
    resultLabel.Text = myCar.FormatDetailsForDisplay();
    ```

- You will find overloaded methods of every single class in the .Net Framework

- You can avoid duplication in the constructors using call chain, borrowing from a previous versions of the overloaded constructor. 

- **this** keyword: this is a member of this class.
- You can use the *this* keyword to remove the duplication. You pass the second version of the constructor to the third overloaded constructor. The third constructor is chained to the second constructor and the second constructor is chained to the first constructor.
    ```cs
    public Car() 
    {

    }

    public Car(string make) : this()
    {
        this.Make = make;
    }

    public Car(string make, int year) : this(string make)
    {
        this.Year = year;
    }
    ```
## CS 044 (Naming conventions for identifiers)

- **PascalCasing**
- **camelCasing**
- Everything **public** will be **PascalCased**.
- Everything **private** will be **camelCased**. 
- **camelCase** for **local scoped variables**.
- **Server control names** use **camelCase** because they're protected (in the designer declaration)
- **Private fields** normally use **underscore at the beginning** to avoid confusions with the names of the input parameters. When we're using input parameters in a constructor we can set the private field equal to the input parameter name by the same name.
    ```cs
    Class Hero{
        private string _name;

        public Hero(string name){
            _name = name;
        }
	}
    ```
## CS 045 (Static vs Instance Members)

- **Static members:** no instance required to use the member. (Stateless)
- **Instance members:** instance required to use the member. They work on stateful means.

- Don't use the static keyword in your own classes unless they're classes that contain helper methods for public use. They rarely have properties.

    ```cs
    public class Valuation
    {
        public static int performCalculation(int val1, int val2)
        {
            return val1 * val2;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int value = Valuation.performCalculation(4, 5);
        resultLabel.Text = String.Format("Resultado: {0}", value);
    }
    ```
- If you call a helper method inside the static method the helper method has to be static as well. Inside a static method we don't have *this*,  so we cannot use the properties, even if they're public.

- You can mark an entire class as static. 

## CS 046 (Lists)

- Collections are strongly-typed fashion. You can know for certain what datatype is.
- Performance is generally better than with arrays, especially if you have to insert elements in the beggining and in the middle.
- Dictionary: you have a key and a value.
- You can use more functionalities with the LINQ (language integrated query) extension methods.
- **List<T>:** Generic collection specif one datatype
- **Dictionary<TKey, TValue>**
- **LinkedList:** makes insertion at various points inside of the list easier.
- **Hashset:** allows to find duplicates of data collections. 
- Add cars to a collection:
    ```cs
    string result = "";
    Car car1 = new Car("BMW", 1986);
    Car car2 = new Car("Porsche", 1985);
    Car car3 = new Car("Chevrolet", 1983);

    List<Car> cars = new List<Car>();
    cars.Add(car1);
    cars.Add(car2);
    cars.Add(car3);
    ```
-  To get the properties of the class car you can easily iterate like:
    ```cs
    for (int i = 0; i < cars.Count; i++)
        result += cars.ElementAt(i).Make + "<br/>";

    resultlabel.Text = result;
    ```
- **FindAll** method:
    ```cs
    List<Car> carsModelo85 = cars.FindAll(p => p.Year == 1985);
    ```
- To change the make of every element in the collection:
    ```cs
    cars.ForEach(p => p.Make = "Honda");
    ```
## CS 047 (Object initializers)

- If the programmer didn't create an overloaded constructor to initialize the class you can insert an element of a list and instantiate at the same time: 
    ```cs
    List<Car> cars = new List<Car>();
    cars.Add(new Car { Make = "BMW", Model = "M5", Year = 1986,Color = "Blue" });
    ```
## CS 048 (Collection initializers)

- Here we are putting the collection into a valid state all in one shot. We dont have an empty list of cars.
    ```cs
    List<Car> cars = new List<Car>()
    {
        new Car { Make = "BMW", Model = "M5", Year = 1986, Color = "Blue" },
        new Car { Make = "Honda", Model = "NSX", Year = 1989, Color = "Red" }
    }; 

    for (int i = 0; i < cars.Count; i++)
    {
        result += cars.ElementAt(i).FormatDetailsForDisplay();
    }
    ```

## CS 049 (Working with the dictionary collection)

- The key has to be a unique number. For example the VIN when you're working with cars:
    ```cs
    string result = "";
    Dictionary<string, Car> cars = new Dictionary<string, Car>() 
    {
        {"V1", new Car { Make = "BMW", Model = "528i", Year = 2010, Color = "Black" } },
        {"V2", new Car { Make = "BMW", Model = "745li", Year = 2005, Color = "Black"} },
        {"V3", new Car { Make = "Ford", Model = "Escape", Year = 2005, Color = "Black" } }
    };
           
    for (int i = 0; i < cars.Count; i++)
    {
        result += "Key: " + cars.ElementAt(i).Key + " Value: " + cars.ElementAt(i).Value.FormatDetailsForDisplay() + "<br/>";
    }

    resultLabel.Text = result;
    ```
-  To retrieve an specific car:
    ```cs
    Car v2;
    if (cars.TryGetValue("V2", out v2))
         result += v2.FormatDetailsForDisplay();
    ```
- To remove an element from the dictionary:
    ```cs
     if (cars.Remove("V2"))
        result += "<h2>Success</h2>";
    ```
## CS 050 (Looping with the foreach statement) 

```cs
List<Car> cars = new List<Car>();
cars.Add(new Car { Make = "BMW", Model = "M5", Year = 1986, Color = "Blue" });
cars.Add(new Car { Make = "Honda", Model = "NSX", Year = 1989, Color = "Red" });

foreach (Car car in cars)
    result += car.FormatDetailsForDisplay();
```

## CS 051 (Implicityly typed local variables using the var keyword)

- When you're using local variables you don't have to give the compiler the type of the variable. It can figure it out based on the value that you use to initialize it.
- You HAVE TO initialize a variable using var keyword:
    ```cs
    var cars = new List<Car>();
    cars.Add(new Car { Make = "BMW", Model = "M5", Year = 1986, Color = "Blue" });
    cars.Add(new Car { Make = "Honda", Model = "NSX", Year = 1989, Color = "Red" });

    foreach (var car in cars)
        result += car.FormatDetailsForDisplay();
    ```
- The variable is now permanently set to that datatype.

## CS 052 (Creating GUIDs)

- Often used in databases.
    ```cs
    //var myGuid = Guid.NewGuid();
    //resultLabel.Text = myGuid.ToString();

    Guid myOtherGuid;
    if (Guid.TryParse("2784ed6b-e544-4cde-88d1-69ec6bc55794", out myOtherGuid))
    {
        resultLabel.Text = myOtherGuid.ToString();
    }
    ```
## CS 053 (Working with enumerations)

- Enumerations will restrict the value of the variable for the options you decided to be for your application. 
- Enumerations is a datatype, you create it. Restricted only to the options that you decide you need.
- You have to avoid magic strings because they provide "bad data".
    ```cs
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Character hero = new Character();
            hero.Name = "Elric";
            hero.Type = CharacterType.Fighter;

            if (hero.Type == CharacterType.Fighter)
            {
                resultLabel.Text = "Our hero is a fighter";
            }
        }
    }

    public class Character
    {
        public string Name { get; set; }
        public CharacterType Type { get; set; }
    }

    public enum CharacterType
    {
        Wizard,
        Fighter,
        Monster
    }   
    ```
-  Check if selected value is an enumeration:
    ```cs  
    var hero = new Character();
    hero.Name = heroNameTextBox.Text;
    CharacterType selection;

    if(Enum.TryParse(heroTypeDropDownList.SelectedValue, out selection))
        hero.Type = selection;
    ```
## CS 053b (Creating Constants with the const Keyword)

- Constants can be created at local level and a global level:
    ```cs
    const int hoursPerDay = 24
 	public class Calendar
    {
        public const int months = 12;
        public const int weeks = 52;
        public const int days = 365;  
    }

    const double daysPerWeek = (double)Calendar.days / (double)Calendar.weeks;
    ```
- Use constants to remove magic values

## CS 054 (Understanding the switch statement)

- Understanding the switch statement.
- It works great with enums.
- Shortcut: sw + Tab + Tab.
    ```cs
    switch (switchExpression)
    {
        case 0:
            result += "Case 0<br/>";
            break;
        case 1:
            result += "Case 1<br/>";
            break;
        default:
            break;
    }
    ```
- **Goto**
    ```cs
    int switchExpression = 1;

    switch (switchExpression)
    {
        case 0:
            result += "Case 0<br/>";
            break;
        case 1:
            // result += "Case 1<br/>";
            // break;
            goto case 0;
        default:
            result += "Default<br/>";
            break;
    }
    switch (switchExpression)
    {
        case 0:
            result += "Case 0<br/>";
            break;
        case 1:
            // result += "Case 1<br/>";
            // break;
            goto case 0;
        case 4 - 2:
            result += "Case 2<br/>";
            return;
        default:
            result += "Default<br/>";
            break;
    }
    ```
-  To exit a case you can return, break or throw exception.
- Using switch with enumerations: sw + tab + tab -> (hero.Type) + Return + Return creates the switch structure for the enumeration:
    ```cs
    switch (hero.Type)
    {
        case CharacterType.Wizard:
            break;
        case CharacterType.Fighter:
            break;
        case CharacterType.Monster:
            break;
        default:
            break;
    }
    ```
## CS 055 (Peparation of concerns principle)

- Some people say that no class should run of more than the size of the screen.

- Am I trying to do to much in this class?

- **Separation of concerns:** you separate the concerns of your application in an effort to make it more resilient to change. We can mitigate the impact of change by separating the concerns of an application. 

- **Presentation layer:** display logic, how do I take data and display it to the user in a certain way?

- **Domain layer:** business domain or the problem domain. Business logic.

- **Persistence layer:** permanent data storage and the retrieval.

- You technically should be able to replace the database that you use without affecting the domain layer. 

- Separation of concerns as a guidance in where code belongs in your application. 

- Useful for Unit testing. 

- Create blank solution in other projects -> Add new project (SomeApp.Web) -> Add new project, class library (SomeApp.Domain) -> Add new project, class library (SomeApp.persistence), for the data
 
- You can separate the implementation of an API in a different project to mitigate the changes in that implementation providing only public methods and property to access them.  In case you have to change the API implementation or use another API you dont have to redesign the whole project. (For  example fb API, Paypal API, etc.)

## CS 056 (Understanding exception handling)

- Runtime exceptions. 
- Base class Exception 
- You can add multiple catch statements, catching the most specific first. 
    ```cs
    catch(DivideByZeroException)
    {
        result = "Please enter a value greater than zero for games pleyed.";
    }
    catch (Exception ex)
    {
        result = "There was a problem: " + ex.Message;
        //throw;
    }
    ```
- To directly throw an exception
    ```cs
    if (attacker.HitPoints <= 0)
            throw new ArgumentOutOfRangeException("Attacker is dead and cannot attack.");
    if (defender.HitPoints <= 0)
            throw new ArgumentOutOfRangeException("Defender is already dead.");
            /*There was a problem: Specified argument was out of the range of valid values. Parameter name: Defender is already dead.*/
    ```
- The program throws which is the message of the ex.Message

- You have to add a try catch in those parts of your application where you do not have control. (User inputs, database access, etc)

- You dont have to return anything to show successful. You have to send exception or otherwise assume success. 

- Asp.net validation controls.
- You have to log your exceptions.
- **Finally** to close connections. 

## CS 057 (Global exception handling)

-  Handle exception from the global perspective:
- Code that fires off at a higher level in Global.asax 
    ```cs
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            var innerException = ex.InnerException;
            Response.Write("<h2>Something bad happened...</h2>");
            Response.Write("<p>" + innerException.Message + "</p>");
            //you need to clear the error at the end to avoid the yellow screen of death

            //Add logging method here!!
            Server.ClearError();
        }

    }
    ```

- Handle specific exceptions handled globally.
- **Server.Redirect:** tells the web browser to go to a different page.
- **Server.Transfer:** take the current session and everything and say deliver this page instead. It shows the second page but the url remains in the current page.
    ```cs 
    void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            var innerException = ex.InnerException;

            if (innerException.GetType() == typeof(ArgumentOutOfRangeException))
                Server.Transfer("Error.aspx");

            Response.Write("<h2>Something bad happened...</h2>");
            Response.Write("<p>" + innerException.Message + "</p>");
            Server.ClearError();
        }
    ```
- You can get the server last error in the error page:
    ```cs
    protected void Page_Load(object sender, EventArgs e)
    {
        var ex = Server.GetLastError();
        if (ex != null)
            resultLabel.Text = "Error: " + ex.InnerException.Message;
        else
            resultLabel.Text = "Something went horribly wrong, but it wasn't your fault.";
    }
    ```
 ## CS 059 (Connecting to a db)

- Add new item -> Sql database (.mdf)
- Open -> Create table structures 
- VARCHAR(MAX)
- Create the database structure and click UPDATE 
- To insert some records just go to the Server Explorer, double click on the table and right click show table data. 

## CS 060 (Creating an entity model)

- Entity framework: API from .NET to access data and manipulate it using C#. It's an object-relational mapper.
- It makes it feel like you are working with any other object. Entity data model will give us entities that represent the data. 
- Abstraction layer.
- New project -> Add new item -> Data -> ADO.Net entity data model -> Select tables 
- You can place the connection string and other settings and other settings in the web.config in order to make changes without compiling the solution. 
- .tt files generated as template files that will generate a series of classes. It creates:
    1. **Context class:** A handle to the database. Logic to connect to the db and perform the actions that we request.
    2. It generates the class Customer based on our table columns.
        ```cs
        protected void Page_Load(object sender, EventArgs e)
        {
            ACMEEntities db = new ACMEEntities();
            var customers = db.Customers;

            string result = "";
            foreach (var customer in customers)
            {
                result += "<p>" + customer.Name + "</p>";
            }

            resultLabel.Text = result;   
        }
        ```
## CS 061 (Display the DBSet Result in a GridView)

- To bind entity to a gridview:
    ```cs
    ACMEEntities db = new ACMEEntities();
    var customers = db.Customers;

    customersGridView.DataSource = customers.ToList();
    customersGridView.DataBind();
    ```
- Separate projects for each layer in your solution.

- Tools driven architecture vs mantainence driven architecture. 

## CS 062 Implementing button command in gridview

- Gridview de-select autogenerate columns

- BoundField Column, allows you to bind one property from an instance from the collection to this column. Add as many columns as required and add the HeaderText and the DataField (name of the property).

- You can add a button column property, to handle the click event go to the RowCommand event.
    ```cs
    protected void carGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // e.CommandArgument = 
        int index = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = carGridView.Rows[index];
        //Breaks if you change the columns order
            
        var make = row.Cells[1].Text;
        var model = row.Cells[2].Text;
        var value = row.Cells[4].Text;
        var carId = Guid.Parse(value);

        resultLabel.Text = String.Format("{0} {1} {2}", make, model, carId);
    } 
    ```
## CS 064 Using mantainance-centric approach

- Create a Persistence Project and a Domain Project, add database and entity model to the Persistence project.
- In the LocalDBExample.Persistence/CustomersRepository.cs add:
    ```cs
    namespace LocalDBExample.Persistence
    {
        public class CustomersRepository
        {
            public static List<Customer> GetCustomers()
            {
                ACMEEntities db = new ACMEEntities();
                var dbCustomers = db.Customers.ToList();
                return dbCustomers;
            }
        }
    }
    ```
- Add in the **domain layer**:
    ```cs
    using LocalDBExample.Persistence;

    namespace LocalDBExample.Domain
    {
        public class CustomerManager
        {
            public static List<Customer> GetCustomers()
            {
                var customers = CustomersRepository.GetCustomers();
                return customers;
            }
        }
    } 
    ```
- We need to add a data transfer object layer. All it does is define a POCO (plain old clr object). This allows easy transfer of data from one layer to another. 

- Add new project named LocalDBExample.DTO. Copy the code that was created automatically with the entity model. Copy that code in the DTO class. 

- You have to add the reference to the POCO in all the layers (Domain, Persistence and Presentation)

- In the persistence layer fill the POCO with the data retrieved from the entity model.
    ```cs
    namespace LocalDBExample.Persistence
    {
        public class CustomersRepository
        {
            public static List<DTO.Customer> GetCustomers()
            {
                ACMEEntities db = new ACMEEntities();
                var dbCustomers = db.Customers.ToList();
                var dtoCustomers = new List<DTO.Customer>();

                foreach (var dbCustomer in dbCustomers)
                {
                    var dtoCustomer = new DTO.Customer();
                    dtoCustomer.CustomerId = dbCustomer.CustomerId;
                    dtoCustomer.Name = dbCustomer.Name;
                    dtoCustomer.PostalCode = dbCustomer.PostalCode;
                    dtoCustomer.City = dbCustomer.City;
                    dtoCustomer.Address = dbCustomer.Address;
                    dtoCustomer.Notes = dbCustomer.Notes;
                    dtoCustomers.Add(dtoCustomer);
                }
                return dtoCustomers;
            }
        }
    }
    ```
- In the Domain layer now we have to retrieve the list of the DTO.Customers instead of the Customers directly.
    ```cs
    using LocalDBExample.Persistence;

    namespace LocalDBExample.Domain
    {
        public class CustomerManager
        {
            public static List<DTO.Customer> GetCustomers()
            {
                var customers = CustomersRepository.GetCustomers();
                return customers;
            }
        }
    }
    ```
- In the presentation call the Domain layer:
    ```cs
    protected void Page_Load(object sender, EventArgs e)
    {
        var customers = Domain.CustomerManager.GetCustomers();
        customersGridView.DataSource = customers.ToList();
        customersGridView.DataBind();
    }
    ```
- Now we separated our concerns. Now we can remove the persistence layer or any other layer and everything should work fine. Applying single responsability. Now we can introduce Unit testing.

## CS 065 (Creating a New Instance of an Entity and Persisting to the Database)

- To save customers to the database you have to add the following method in the persistence layer (get a DTO customer and add it to the customer object in the entity model, then save the changes):

    ```cs
    public static void AddCustomer(DTO.Customer newCustomer)
    {
        ACMEEntities db = new ACMEEntities();
        var dbCustomers = db.Customers;
        var customer = new Customer();

        if (newCustomer.Name.Trim().Length == 0)
            throw new Exception("Name is a required field");
            //other validations here...
        customer.CustomerId = newCustomer.CustomerId;
        customer.Name = newCustomer.Name;
        customer.Address = newCustomer.Address;
        customer.PostalCode = newCustomer.PostalCode;
        customer.City = newCustomer.City;
        customer.Notes = newCustomer.Notes;
        try
        {
            dbCustomers.Add(customer);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
                //Log exception
            throw ex;
        }
    }

        //A custom exception may be created in the domain layer.
    public static void AddCustomer(DTO.Customer customer)
    {
        try
        {
            Persistence.CustomersRepository.AddCustomer(customer);
        }
        catch (Exception ex)
        {
            throw ex;
        } 
    }
    ```
- In the presentation layer
    ```cs
    protected void okButton_Click(object sender, EventArgs e)
    {
        var newCustomer = new DTO.Customer();
        newCustomer.CustomerId = Guid.NewGuid();
        newCustomer.Name = nameTextBox.Text;
        newCustomer.PostalCode = zipTextBox.Text;
        newCustomer.City = cityTextBox.Text;
        newCustomer.Address = addressTextBox.Text;
        newCustomer.Notes = notesTextBox.Text;
        try
        { 
            Domain.CustomerManager.AddCustomer(newCustomer);
            displayCustomer();
        }
        catch (Exception ex)
        {
            resultLabel.Text = ex.Message;
        }
    }
        //To order the info from the database before displaying it:
    ACMEEntities db = new ACMEEntities();
    var dbCustomers = db.Customers.OrderBy(p=> p.Name).ToList();
    ```
## CS 066 (Package Management with NuGet)

- **NuGet:** Package manager. It downloads the actual packages and posible package dependencies. 

- You can use the GUI integrated with Visual Studio or the the Package Manager Console. 

- To use the console: Tools - Nuget - Package management console

- When you use nuget it adds to your project a packages.config file. 
    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <packages>
    <package id="bootstrap" version="3.3.7" targetFramework="net452" />
    <package id="elmah" version="1.2.2" targetFramework="net452" />
    <package id="elmah.corelibrary" version="1.2.2" targetFramework="net452" />
    <package id="jQuery" version="3.1.1" targetFramework="net452" />
    <package id="Microsoft.CodeDom.Providers.DotNetCompilerPlatform" version="1.0.5" targetFramework="net452" />
    <package id="Microsoft.Net.Compilers" version="2.2.0" targetFramework="net452" developmentDependency="true" />
    </packages>
    ```
- There is also a packages folder, outside the project. 

## CS 067 

- Add the package.
- Tools -> Options -> NuGet package manager -> General, check if options of package restore are checked.
- In the solution Enable nuget package restore.

## CS 068 (Bootstrap)

- Add it with NuGet
    ```html
    <div class="page-header">
        <h1>Page header</h1>
        <p class="lead">By line (lead)</p>
    </div>      
    ```
- The grid:
    ```html
    <div class="row">
                <div class="col-md-8 col-sm-6">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc enim lacus, consectetur eget tellus at, dapibus fringilla erat. Sed in nunc lectus. Morbi sit amet lorem pellentesque metus auctor consectetur. Integer ultrices mauris non lacus congue bibendum. Cras maximus feugiat ligula, id gravida orci elementum varius. Pellentesque at orci ac dolor lacinia convallis ut eget purus. In hac habitasse platea dictumst. Vivamus in elit in ex lobortis tincidunt non at massa. Pellentesque bibendum sodales erat, in congue nisi blandit dictum. Donec facilisis lacus vel efficitur sollicitudin. Praesent dictum gravida nisi, quis scelerisque metus ornare et. Morbi turpis ligula, cursus sit amet imperdiet et, dignissim quis est.
                </div>
                <div class="col-md-4 col-sm-6">
                    Pellentesque eget ipsum nibh. Donec ultrices dictum justo a eleifend. Vivamus tincidunt nulla urna, id ultricies est varius id. Pellentesque facilisis malesuada elementum. Integer ac dui sit amet eros congue lobortis a in dolor. Donec dignissim blandit neque nec condimentum. Nam pulvinar est odio. Proin eleifend arcu nibh, nec fringilla diam ornare et. Aliquam hendrerit convallis rhoncus. Cras varius egestas quam, sit amet molestie metus feugiat id.
                </div>
        </div>
    ```
- To style a form:
- One form-group for each server control!
    ```html
     <div class="form-group">
            <label for="testTextBox">Textbox</label>
            <asp:TextBox ID="testTextBox" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
            //Same format for radio button
         <div class="checkbox"><label><asp:CheckBox ID="CheckBox1" runat="server" /> Checkbox</label> </div>
    ```
- You can add a button control and change the cssclass to use the bootstrap styles. 
    ```html
    <asp:Button ID="Button1" runat="server" Text="Button" CssClass="btn btn-lg btn-primary"/>
    ```
## CS 069 (Mapping enum types to entity properties)
- Create the enums that you want to map
    ```cs
    public enum Color
    {
        Black,
        Red,
        Silver
    }

    public enum ProductType
    {
        Longboard,
        Skateboard,
        Helmet
    }
    ```
- Create an entity model base on a .mdf database.
Right click in the property in the Entity model diagram and -> convert to enum -> Reference external file (Namespace.Type) ->
EntityFrameworkEnum.ProductType (it changes the properties datatype from integer to ProductType). It also changes the Product definition (Product.cs)
    ```cs
    public System.Guid ProductId { get; set; }
    public string Name { get; set; }
    public EntityFrameworkEnums.ProductType ProductType { get; set; }
    public EntityFrameworkEnums.Color ProductColor { get; set; }
    ```
- If you dont have an Enum previously created you can create in convert to enum and add the value members. If you do it like this the entity framework will create a Enum.cs class with the properties.

## Interview questions:

1. IEnumerable vs IList?
    - Both are interfaces and are part of System.Collection.
    - IList inherits from IEnuerable (IList is extended version of IEnumerable). IList has Add, Remove, Contains, Count
    - Both have deffered execution style (query will not give results until enumerated).
    - IEnumerable is for read-only approach. It has further filter support. 
    - IEnumerable has IEnumerator interface and GetEnumerator method.
    - Both execute select query on the server side, loads data in-memory on the client-side and then filters.
2. IEnumerable vs List?
    - List is more efficient when you need to enumerate the data multiple times (cached). IEnumerable is more efficient if you need to enumerate only once.
3. IEnumerable vs IQueryable?
    - IQueryable exist in System.Linq namespaces, IEnuerable in System.Collection.
    - IQueryable is suitable for querying data from out-memory collections (like remote db or service). IEnumerable is better for In-memory collection query.
    - IQueryable executes select query and filters on server-side (only the required records are sent from the db). 
    - IQueryable is beneficial for LINQ to SQL, whereas IEnuerable is beneficial for Linq to Object. 
    - Both support deffered execution but IEnumerable doesn't support lazy loading (not suitable for paging like scenarios).
    - IQueryable supports custom query using CreateQuery and Execute methods.
    - IQueryable inherits from IEnumerable.
    - IQueryable reduces network traffic and uses the power of SQL language (server-side filtering).
4. Different types of errors?
    - Syntax errors
    - Runtime errors
    - Logical errors (harder to track)
5. FOR...NEXT loop? It is known in advance how many times the loop must be repeated. 
6. Compiler? Unique program that can process statements which are written in a particular programming language and can turn them into machine language.
7. Can multiple catch blocks be executed? No. Once the proper catch code executed the control is transferred to the finally block. 



 

    




   		 


       