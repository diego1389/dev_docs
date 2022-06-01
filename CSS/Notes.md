- CSS in line style, inside style tags and using an external .css file called using a <link> tag.
```css
h1, h2, h3, h4, p, li{ /*group multiple selectors*/
    font-family: sans-serif;
}

h1{
    color: blue;
    font-size: 26px;
    font-family: sans-serif;
    text-transform: uppercase;
    font-style: italic;
}

footer p{ /*All the p tags inside footer*/
    font-size: 14px;
}

h2{
    font-size: 40px;
}

h3{
    font-size: 30px;
}

h4{
    font-size: 20px;
    text-transform: uppercase;
    text-align: center;
}

p{
    font-size: 22px;
    line-height: 1.5;/*space between paragraph lines*/
}

li{
    font-size: 20px;
}
```
### Class and id selectors:

- Command (Ctrl) + / to comment a code block.
- We cannot repeat id names but we can reuse classes. 

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <title>The Basic Language of the Web: HTML</title>
    <link rel="stylesheet" type="text/css" href="styles.css"/>
  </head>

  <body>
    <!--
    <h1>The Basic Language of the Web: HTML</h1>
    <h2>The Basic Language of the Web: HTML</h2>
    <h3>The Basic Language of the Web: HTML</h3>
    <h4>The Basic Language of the Web: HTML</h4>
    <h5>The Basic Language of the Web: HTML</h5>
    <h6>The Basic Language of the Web: HTML</h6>
    -->

    <header>
      <h1>📘 The Code Magazine</h1>

      <nav>
        <a href="blog.html">Blog</a>
        <a href="#">Challenges</a>
        <a href="#">Flexbox</a>
        <a href="#">CSS Grid</a>
      </nav>
    </header>

    <article>
      <header>
        <h2>The Basic Language of the Web: HTML</h2>

        <img
          src="img/laura-jones.jpg"
          alt="Headshot of Laura Jones"
          height="50"
          width="50"
        />

        <p id="author">Posted by <strong>Laura Jones</strong> on Monday, June 21st 2027</p>

        <img
          src="img/post-img.jpg"
          alt="HTML code on a screen"
          width="500"
          height="200"
        />
      </header>

      <p>
        All modern websites and web applications are built using three
        <em>fundamental</em>
        technologies: HTML, CSS and JavaScript. These are the languages of the
        web.
      </p>

      <p>
        In this post, let's focus on HTML. We will learn what HTML is all about,
        and why you too should learn it.
      </p>

      <h3>What is HTML?</h3>
      <p>
        HTML stands for <strong>H</strong>yper<strong>T</strong>ext
        <strong>M</strong>arkup <strong>L</strong>anguage. It's a markup
        language that web developers use to structure and describe the content
        of a webpage (not a programming language).
      </p>
      <p>
        HTML consists of elements that describe different types of content:
        paragraphs, links, headings, images, video, etc. Web browsers understand
        HTML and render HTML code as websites.
      </p>
      <p>In HTML, each element is made up of 3 parts:</p>

      <ol>
        <li>The opening tag</li>
        <li>The closing tag</li>
        <li>The actual element</li>
      </ol>

      <p>
        You can learn more at
        <a
          href="https://developer.mozilla.org/en-US/docs/Web/HTML"
          target="_blank"
          >MDN Web Docs</a
        >.
      </p>

      <h3>Why should you learn HTML?</h3>

      <p>
        There are countless reasons for learning the fundamental language of the
        web. Here are 5 of them:
      </p>

      <ul>
        <li>To be able to use the fundamental web dev language</li>
        <li>
          To hand-craft beautiful websites instead of relying on tools like
          Worpress or Wix
        </li>
        <li>To build web applications</li>
        <li>To impress friends</li>
        <li>To have fun 😃</li>
      </ul>

      <p>Hopefully you learned something new here. See you next time!</p>
    </article>

    <aside>
      <h4>Related posts</h4>

      <ul class="related-posts-list">
        <li>
          <img
            src="img/related-1.jpg"
            alt="Person programming"
            width="75"
            width="75"
          />
          <a href="#">How to Learn Web Development</a>
          <p class="related-author">By Jonas Schmedtmann</p>
        </li>
        <li>
          <img src="img/related-2.jpg" alt="Lightning" width="75" heigth="75" />
          <a href="#">The Unknown Powers of CSS</a>
          <p class="related-author">By Jim Dillon</p>
        </li>
        <li>
          <img
            src="img/related-3.jpg"
            alt="JavaScript code on a screen"
            width="75"
            height="75"
          />
          <a href="#">Why JavaScript is Awesome</a>
          <p class="related-author">By Matilda</p>
        </li>
      </ul>
    </aside>

    <footer>
      <p id="copyright">Copyright &copy; 2027 by The Code Magazine.</p>
    </footer>
  </body>
</html>
```
- style.css
```css
h1, h2, h3, h4, p, li{
    font-family: sans-serif;
}

h1{
    color: blue;
    font-size: 26px;
    font-family: sans-serif;
    text-transform: uppercase;
    font-style: italic;
}

/* footer p{
    font-size: 14px;
} */

h2{
    font-size: 40px;
}

h3{
    font-size: 30px;
}

h4{
    font-size: 20px;
    text-transform: uppercase;
    text-align: center;
}

p{
    font-size: 22px;
    line-height: 1.5;/*space between paragraph lines*/
}

li{
    font-size: 20px;
}
/*
#article header p{
    font-style: italic;
}
*/
#author{ /*id selector*/
    font-style: italic;
    font-size: 18px;
}

#copyright{
    font-size: 14px;
}

.related-author{ /*class selector*/
    font-size: 18px;
    font-weight: bold;
}

.related-posts-list{
    list-style: none;
}
```

### Working with colors

- RGB notation and RGBA for transparency (alpha value).
- Hexadecimal notation. It goes from 00 to ff (255).
- Cyan. RGB = rgb(0, 255, 255). Hexadecimal = #00ffff. Hexadecimal shorthand: #0ff.
- There are 256 pure grays to choose from. Grays occur when all the r g and b values are the same (except for 255 white and 0 black). F.e: rgb(69,69,69) / #444
- When you have more than one definition it will apply the last one. 
- index.html
```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <title>The Basic Language of the Web: HTML</title>
    <link rel="stylesheet" type="text/css" href="styles.css"/>
  </head>

  <body>
    <!--
    <h1>The Basic Language of the Web: HTML</h1>
    <h2>The Basic Language of the Web: HTML</h2>
    <h3>The Basic Language of the Web: HTML</h3>
    <h4>The Basic Language of the Web: HTML</h4>
    <h5>The Basic Language of the Web: HTML</h5>
    <h6>The Basic Language of the Web: HTML</h6>
    -->

    <header class="main-header">
      <h1>📘 The Code Magazine</h1>

      <nav>
        <a href="blog.html">Blog</a>
        <a href="#">Challenges</a>
        <a href="#">Flexbox</a>
        <a href="#">CSS Grid</a>
      </nav>
    </header>

    <article>
      <header>
        <h2>The Basic Language of the Web: HTML</h2>

        <img
          src="img/laura-jones.jpg"
          alt="Headshot of Laura Jones"
          height="50"
          width="50"
        />

        <p id="author">Posted by <strong>Laura Jones</strong> on Monday, June 21st 2027</p>

        <img
          src="img/post-img.jpg"
          alt="HTML code on a screen"
          width="500"
          height="200"
        />
      </header>

      <p>
        All modern websites and web applications are built using three
        <em>fundamental</em>
        technologies: HTML, CSS and JavaScript. These are the languages of the
        web.
      </p>

      <p>
        In this post, let's focus on HTML. We will learn what HTML is all about,
        and why you too should learn it.
      </p>

      <h3>What is HTML?</h3>
      <p>
        HTML stands for <strong>H</strong>yper<strong>T</strong>ext
        <strong>M</strong>arkup <strong>L</strong>anguage. It's a markup
        language that web developers use to structure and describe the content
        of a webpage (not a programming language).
      </p>
      <p>
        HTML consists of elements that describe different types of content:
        paragraphs, links, headings, images, video, etc. Web browsers understand
        HTML and render HTML code as websites.
      </p>
      <p>In HTML, each element is made up of 3 parts:</p>

      <ol>
        <li>The opening tag</li>
        <li>The closing tag</li>
        <li>The actual element</li>
      </ol>

      <p>
        You can learn more at
        <a
          href="https://developer.mozilla.org/en-US/docs/Web/HTML"
          target="_blank"
          >MDN Web Docs</a
        >.
      </p>

      <h3>Why should you learn HTML?</h3>

      <p>
        There are countless reasons for learning the fundamental language of the
        web. Here are 5 of them:
      </p>

      <ul>
        <li>To be able to use the fundamental web dev language</li>
        <li>
          To hand-craft beautiful websites instead of relying on tools like
          Worpress or Wix
        </li>
        <li>To build web applications</li>
        <li>To impress friends</li>
        <li>To have fun 😃</li>
      </ul>

      <p>Hopefully you learned something new here. See you next time!</p>
    </article>

    <aside>
      <h4>Related posts</h4>

      <ul class="related-posts-list">
        <li>
          <img
            src="img/related-1.jpg"
            alt="Person programming"
            width="75"
            width="75"
          />
          <a href="#">How to Learn Web Development</a>
          <p class="related-author">By Jonas Schmedtmann</p>
        </li>
        <li>
          <img src="img/related-2.jpg" alt="Lightning" width="75" heigth="75" />
          <a href="#">The Unknown Powers of CSS</a>
          <p class="related-author">By Jim Dillon</p>
        </li>
        <li>
          <img
            src="img/related-3.jpg"
            alt="JavaScript code on a screen"
            width="75"
            height="75"
          />
          <a href="#">Why JavaScript is Awesome</a>
          <p class="related-author">By Matilda</p>
        </li>
      </ul>
    </aside>

    <footer>
      <p id="copyright">Copyright &copy; 2027 by The Code Magazine.</p>
    </footer>
  </body>
</html>
```
- style.css
```css
h1, h2, h3, h4, p, li{
    font-family: sans-serif;
    color: #444;
}
h1, h2, h3{
    color: #1098ad;
}

h1{
    font-size: 26px;
    font-family: sans-serif;
    text-transform: uppercase;
    font-style: italic;
}

/* footer p{
    font-size: 14px;
} */

h2{
    font-size: 40px;
}

h3{
    font-size: 30px;
}

h4{
    font-size: 20px;
    text-transform: uppercase;
    text-align: center;
}

p{
    font-size: 22px;
    line-height: 1.5;/*space between paragraph lines*/
}

li{
    font-size: 20px;
}
/*
#article header p{
    font-style: italic;
}
*/
#author{ /*id selector*/
    font-style: italic;
    font-size: 18px;
}

#copyright{
    font-size: 14px;
}

.related-author{ /*class selector*/
    font-size: 18px;
    font-weight: bold;
}

.related-posts-list{
    list-style: none;
}

.main-header{
    background-color: #eceaea;
}

aside{
    background-color: #eceaea;
    /*border: 5px solid #1098ad; border is a short hand property because you can assign multiple properties to it*/
    border-top: 5px solid #1098ad; 
    border-bottom: 5px solid #1098ad;
}

/* body{
    background-color: red;
} */
```
### Pseudo-classes

```css
li:first-child{ /*sub classes selectors gets the first element of a container*/
    font-weight: bold;
}
li:last-child{
    font-style: italic;
}
li:nth-child(2){
    color: red;
}

li:nth-child(even){ /*even*/
    color: green;
}
```
### Styling hyperlinks 

- a:link targets all the anchors that have an href attribute. 
```css
/*styling links*/
a:link{
    color: #1098ad;
    text-decoration: none;
}

a:visited{
    color: #1098ad;
}

a:hover{
    color: orangered;
    font-weight: bold;
    text-decoration: underline orangered;
}

a:active{
    background-color: black;
    font-style: italic;
}
```

### Developer tools
### Conflicts between selectors
- All the rules are applied but if there is a conflict in a property there is a certain hierarchy. From highest to lowest priority:
1. Declarations marked !import (try not to use it)
2. Inline style (shouldn't do it).
3. Id selectors. If there is more than one applies the last one.  (#)
4. Class selectors or pseudo class selector. (:) If there is more than one applies the last one. 
5. Element selector (p, li, etc). If there is more than one applies the last one. 
6. Universal selector (*)
  ```html
    <footer>
      <p id="copyright" class="copyright text">Copyright &copy; 2027 by The Code Magazine.</p>
    </footer>
  ```
  ```css
  /*Ti applies font-size 14px and color yellow because text class comes last*/
  #copyright{
    font-size: 14px;
  }

  .copyright{
      font-size: 18px;
      color: blue;
  }

  .text{
      color: yellow;
  }

  footer p{
      color: green;
  }
  ```

### Inheritance
- If we set some properties (only certain properties like text and color, it doesn't work for border for example) in the body it will inherit them to the page's elements. Then it will get overriden when necesary using classes selectors, ids selectors, elements selectors, pseudo classes, etc. 
```css
body{
    color: #444;
    font-family: sans-serif;
}

h1, h2, h3{
    color: #1098ad;
}
```
- Universal selector (*) will get inherithed by all the elements (body selector only works for certain properties like text and color). It is the one with the lowest priority. 

### The CSS box model
- Content: (text, images, etc) width and height area.
- Border: (inside of the element).
- Padding: invisible space inside of the element, it is around the content.
- Margin: space outside of the element, between elements. 
- Fill area: some properties (like background image) include not only the content area but also the margin and the padding. That's the fill area. 

- Final element width = left border + left padding + width + right padding + right border
- Final element height = top border + top padding + height + bottom padding + bottom border. 
```css
.main-header{
    background-color: #eceaea;
    /*padding: 20px;*/
    padding: 20px 40px;/*20px top and bottom and 40 px left and right*/
}
```
- Add margin between list elements but not between the last li element and the bottom section:
```css
li{
    font-size: 20px;
    margin-bottom: 10px;
}

li:last-child{
    margin-bottom: 0;
}
```
- Reset the page margin and padding at the beginning of the project:
```css
* {
    margin: 0;
    padding: 0;
}
```
- When setting space between elements stick to one side (top or bottom). Don't mix them. For example set only margin-bottom for all the elements. 
- Collapsing margins: when we have two margins that occupy the same space only one of them is visible on the page. 
- Centering our page. Place everything inside a container (div). 
```css
.container{
    width: 700px;
    margin-left: auto;
    margin-right: auto;
    margin: 0 auto;
}
```
### Types of boxes
- Block level boxes occupy all the space horizontally. 
  - Are formatted visually as blocks.
  - Ocuppy 100 % of parent element's width, no matter the content.
  - Elements are stacked vertically by default, one after another.
  - Most of the elements are block-level boxes: body, main, header, footer, section, nav, div, h1-h6, p, ul, ol, li, etc.
  - To transform an inline element to a block element:
    ```css
    display: block;
    ```
- Inline boxes:
  - Occupies only the space necessary for its content.
  - Causes no line breaks after or before the element. 
  - Box model applies in a different way: heights and widths do not apply.
  - Paddings and margins are applied only horizontally (left and right).
  - Some elements: a, img, strong, em, buttom, etc.
  - To transform a block element into an inline element:
    ```css
    display: inline;
    ```
- Inline-block boxes:
  - Look ike inline from the outside but behaves like block-level on the inside.  
  - Occupies only content's space.
  - Causes no line-breaks.
  - Box model applies (can apply height, width, margin and padding). 

     ```css
    nav a:link{
        margin-right: 30px;
        margin-top: 10px ;
        display: inline-block;
    }

    nav a:link:last-child{
        margin-right: 0;
    }
    ```
  - Images are inline-block elements. 
  ### Absolute positioning

  - Normal flow:
    - Default positioning.
    - Element is in flow.
    - Elements are laid out according to their order in the html code. 
    - Default position: position relative. 
  - Absolute positioning:
    - position: absolute.
    - out of flow.
    - No impact on surronding elements (might overlap them).
    - We use top, bottom, left or right to offset the element from its relatively positioned container. 
    - It is position in relation to the viewport. 
    - To absolute position an element we need to make position relative in the parent (the element we want our element to be absolutelity positioned from). 
    ```css
    body{
      color: #444;
      font-family: sans-serif;
      position: relative;
    }

    button{
        font-size: 22px;
        padding: 20px;
        cursor: pointer;
        position: absolute;
        bottom: 50px;
        right: 50px;
    }
    ```

  ### Pseudo elements:
  - Pseudo elements are elements that don't exist in the html but we can select and style using CSS. F.e: first letter of a paragraph or first line, etc. 
  - Pseudo classes are selected with one : and pseudo elements are selected with two ::
  ```css
  h1::first-letter{
    font-style: normal;
    margin-right: 5px;
  }

  p::first-line{
    color: red;
  }
  ```
  - Sibling selector: they belong to the same parent element. 
  - Adjancent selector: the sibling that comes right after the one that you're selecting. 
  - Select the paragraphs that come after h3:
  ```css
  h3 + p::first-line{
    color: red;
  }
  ```
  - After pseudo-element (create an html element through css):
  ```css
  h2{
      font-size: 40px;
      margin-bottom: 30px;
      position: relative;
  }

  h2::after{
      content: "TOP";
      background-color: #ffe70e;
      color: #444;
      font-size: 16px;
      font-weight:  bold;
      display: inline-block;
      padding: 5px 10px;
      position: absolute;
      top: -15px;
      right: -25px;
  }
  ```

  ## Layouts: floats, flexbox and CSS grid

  ### The way of building layouts

  - The way text, images and other content is placed and arranged on a webpage.
  - Page layout gives the page a visual structure into which we place our content. 
  - Component layout: made of smaller pieces of content that have to be arranged in a certain way.
  - Float layouts (legacy), flexbox and css grid.
  - Flexbox: 1 dimensional row without using floats. Perfect for component layouts.
  - CSS grid. Two dimentional grid. Perfect for page layouts and complex components.
  ### Using floats:
  ```html
  <img
        src="img/laura-jones.jpg"
        alt="Headshot of Laura Jones"
        class="author-image"
    />

    <p id="author" class="author">Posted by <strong>Laura Jones</strong> on Monday, June 21st 2027</p>  
  ```
  - You can type "lorem" in VS code and it will add some lorem ipsum for you. 
  ```css
  .author-image{
      float: left;
  }

  .author{
      margin-left: 10px;
      margin-top: 10px;
      float:left;
  }
  ```
  - Option + Up or Option + down to move code around. 
  - Collapsing elements if you set float on all the children of an element it looses its height because float makes the elements "external" to the rest of the page. 
  - Float is different than absolute positioning. They are not a positing scheme. Still they are out of flow. Floated elements DO impact surrounding elements, text and inline elements will wrap around the floated element. 
  - The container element will not adjust its height to the element. 
  ### Clear floats

  ```html
    <header class="main-header clear-fix">
        <h1>📘 The Code Magazine</h1>
        <nav>
          <a href="blog.html">Blog</a>
          <a href="#">Challenges</a>
          <a href="#">Flexbox</a>
          <a href="#">CSS Grid</a>
        </nav>
    </header>
  ```

  ```css
  h1{
    float: left;
  }

  nav{
      float: right;
  }

  .clear-fix::after{
      content: '';
      display: block;
      clear:both;
  }
  ```

  ### Build a simple float layout

  - .htmnl
  ```html
  <!DOCTYPE html>
  <html lang="en">
    <head>
      <meta charset="UTF-8" />
      <title>The Basic Language of the Web: HTML</title>
      <link rel="stylesheet" type="text/css" href="styles.css"/>
    </head>

    <body>
      <!--
      <h1>The Basic Language of the Web: HTML</h1>
      <h2>The Basic Language of the Web: HTML</h2>
      <h3>The Basic Language of the Web: HTML</h3>
      <h4>The Basic Language of the Web: HTML</h4>
      <h5>The Basic Language of the Web: HTML</h5>
      <h6>The Basic Language of the Web: HTML</h6>
      -->
      <div class="container">
        <header class="main-header clear-fix">
          <h1>📘 The Code Magazine</h1>
    
          <nav>
            <a href="blog.html">Blog</a>
            <a href="#">Challenges</a>
            <a href="#">Flexbox</a>
            <a href="#">CSS Grid</a>
          </nav>
        </header>
    
        <article>
          <header class="post-header">
            <h2>The Basic Language of the Web: HTML</h2>
    
            <img
              src="img/laura-jones.jpg"
              alt="Headshot of Laura Jones"
              class="author-image"
            />
    
            <p id="author" class="author">Posted by <strong>Laura Jones</strong> on 
              Monday, June 21st 2027</p>
              
            <img
              src="img/post-img.jpg"
              alt="HTML code on a screen"
              width="500"
              height="200"
              class="post-image"
            />
          </header>
    
          <p>
            All modern websites and web applications are built using three
            <em>fundamental</em>
            technologies: HTML, CSS and JavaScript. These are the languages of the
            web.
          </p>
    
          <p>
            In this post, let's focus on HTML. We will learn what HTML is all about,
            and why you too should learn it.
          </p>
    
          <h3>What is HTML?</h3>
          <p>
            HTML stands for <strong>H</strong>yper<strong>T</strong>ext
            <strong>M</strong>arkup <strong>L</strong>anguage. It's a markup
            language that web developers use to structure and describe the content
            of a webpage (not a programming language).
          </p>
          <p>
            HTML consists of elements that describe different types of content:
            paragraphs, links, headings, images, video, etc. Web browsers understand
            HTML and render HTML code as websites.
          </p>
          <p>In HTML, each element is made up of 3 parts:</p>
    
          <ol>
            <li>The opening tag</li>
            <li>The closing tag</li>
            <li>The actual element</li>
          </ol>
    
          <p>
            You can learn more at
            <a
              href="https://developer.mozilla.org/en-US/docs/Web/HTML"
              target="_blank"
              >MDN Web Docs</a
            >.
          </p>
    
          <h3>Why should you learn HTML?</h3>
    
          <p>
            There are countless reasons for learning the fundamental language of the
            web. Here are 5 of them:
          </p>
    
          <ul>
            <li>To be able to use the fundamental web dev language</li>
            <li>
              To hand-craft beautiful websites instead of relying on tools like
              Worpress or Wix
            </li>
            <li>To build web applications</li>
            <li>To impress friends</li>
            <li>To have fun 😃</li>
          </ul>
    
          <p>Hopefully you learned something new here. See you next time!</p>
        </article>
    
        <aside>
          <h4>Related posts</h4>
    
          <ul class="related-posts-list">
            <li>
              <img
                src="img/related-1.jpg"
                alt="Person programming"
                width="75"
                width="75"
              />
              <a href="#">How to Learn Web Development</a>
              <p class="related-author">By Jonas Schmedtmann</p>
            </li>
            <li>
              <img src="img/related-2.jpg" alt="Lightning" width="75" heigth="75" />
              <a href="#">The Unknown Powers of CSS</a>
              <p class="related-author">By Jim Dillon</p>
            </li>
            <li>
              <img
                src="img/related-3.jpg"
                alt="JavaScript code on a screen"
                width="75"
                height="75"
              />
              <a href="#">Why JavaScript is Awesome</a>
              <p class="related-author">By Matilda</p>
            </li>
          </ul>
        </aside>
    
        <footer>
          <!--<p id="copyright" class="copyright text">Copyright &copy; 2027 by The Code Magazine.</p>-->
          <p id="copyright">Copyright &copy; 2027 by The Code Magazine.</p>
        </footer>
      </div>
      <button>❤️ Like</button>
    </body>
  </html>
  ```
  ```css
  article{
    width: 825px;
    float: left;
  }

  aside{
      width: 300px;
      float: right;
  }

  footer{
      clear: both;
  }
  ```

### box-sizing boxes

- By default the padding we add to an element adds to the total width.
- In this case it moved the whole aside to the bottom of the page because we only have space for 300 px for the aside section (aligned to the right of the article section).
```css
aside{
  background-color: #eceaea;
  /*border: 5px solid #1098ad; border is a short hand property because you can assign multiple properties to it*/
  border-top: 5px solid #1098ad; 
  border-bottom: 5px solid #1098ad;
  padding: 50px 40px;
}
```
- Redefining the box so we can specify any padding or border and it does not increase the total width and height of the element:
  ```css
  box-sizing: border-box
  ```
  - Final element width = <del>left border + left padding</del> + width + <del>right padding + right border</del>
  - Final element height = <del>top border + top padding </del>+ height + <del>bottom padding + bottom border</del> 
  ```css
  aside{
    background-color: #eceaea;
    border-top: 5px solid #1098ad; 
    border-bottom: 5px solid #1098ad;
    padding: 50px 40px;
    box-sizing: border-box;
  }
  ```
- We want to apply this border-box box sizing to all the elements of the page so we specify it in the universal selector:
  ```css
  *{
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }
  ```
  ### Introduction to Flexbox

  - Set the display property in an html container to flex to align the elements inside it side by side. 
  ```css
    .container {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 40px;
    margin: 40px;

    /* FLEXBOX */
    display: flex;
  }
  ```
  - The elements inside the container (children) are called the flex items:
  ```html
    <body>
    <div class="container">
      <div class="el el--1">HTML</div>
      <div class="el el--2">and</div>
      <div class="el el--3">CSS</div>
      <div class="el el--4">are</div>
      <div class="el el--5">amazing</div>
      <div class="el el--6">languages</div>
      <div class="el el--7">to</div>
      <div class="el el--8">learn</div>
    </div>
  </body>
  ```
  - Vertically all the flex items are as tall as the biggest element (the biggest height). 
  - To center items vertically:
  ```css
  .container {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 40px;
    margin: 40px;

    /* FLEXBOX */
    display: flex;
    align-items: center;
    justify-content: center;
    /*justify-content: space-between;*/
  }
  ```
## Flexbox overview

- A set of related CSS properties for building 1-dimensional layouts.
- The main idea is that empty space inside a container element can be automatically divided by its child elements.
- Makes it easy to automatically align items to one another inside one parent, both horizontally and vertically. 
- Flexbox is perfect for replacing floats. 
- Flexbox terminology:
  - Flex container (display: flex).
    - gap (default 0 | length)
    - justify-content (default flex-start | flex-end | center). To align main axis
    - align-items (default stretch | flex-start | flex-end | center | baseline). To align cross axis.
    - flex-direction
    - flex-wrap
    -align-content
  - Flex items:
    - align-self (default auto | stretch | flex-start | flex-end | center | baseline)
    - flex-grow
    - flex-shrink
    - flex-basis
    - flex
    - order (controls order of items). 
  - Main axis: direction in which items are layout.
  - Cross axis: perpendicular axis.
  
### Spacing and aligning flex items

- To move the first element to the top (cross-alignment) but leave the rest of the flex items centered aligned:
  ```css
  .el--1 {
    background-color: blueviolet;
    align-self: flex-start;
  }
  .el--2 {
    background-color: orangered;
  }
  .el--3 {
    background-color: green;
    height: 150px;
  }
  .el--4 {
    background-color: goldenrod;
  }
  .el--5 {
    background-color: palevioletred;
  }
  .el--6 {
    background-color: steelblue;
  }
  .el--7 {
    background-color: yellow;
  }
  .el--8 {
    background-color: crimson;
  }

  .container {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 40px;
    margin: 40px;

    /* FLEXBOX */
    display: flex;
    align-items: center;
    justify-content: center;
  }
  ```
- To stretch a flex item (takes all the vertical height as the element with the highest height in the container):
```css
  .el--5 {
    background-color: palevioletred;
    align-self: stretch;
  }
```
- The order of all the elements by default is zero. To move an element to the first position set an order smaller than 0. To move it to the end set an order bigger than 0,
```css
  .el--6 {
    background-color: steelblue;
    order: -1;
  }
```
- To set space between the elements use gap (you can also use margin but better use gap). Gap doesn't add margin to the flex items;
```css
 .container {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 40px;
    margin: 40px;

    /* FLEXBOX */
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 30px;
    /*justify-content: space-between;*/
  }
```

### The flex property

- To size flex items. 
- Default values:
```css
.el{
  flex-grow: 0;
  flex-shrink: 1;
  flex-basis: auto;
}
```
- Flex-basis: instead of the width property. When elements have content bigger it extends to display all the content. It is like a recommendation to the browser but it is not a rigid width. If there is not enough room in the container it can "balance" the width between the elements. 
- That can be changed with the flex-shrink property. If you set it to 0 it won't fit it the container but it will set the exact value specified in flex-basis property. 
```css
.el{
    flex-basis: 200px;
    flex-shrink: 0;
  }
```
- Flex-grow to specify if elements are allowed to grow as much as they can or not. 
- If you set flex-grow to 1 it will evenly divide the container's space:
```css
.el{
    /* flex-basis: 200px; */
    flex-shrink: 0;
    flex-grow: 1;
  }
```
- You can set flex-grow to just one flex element. It will take all the remaining space (or divide the remaining space evenly between the items that you specify flex-grow:1).
```css
.el--1 {
  background-color: blueviolet;
  align-self: flex-start;
  flex-grow: 1;
}
.el--2 {
  background-color: orangered;
}
.el--3 {
  background-color: green;
  height: 150px;
}
.el--4 {
  background-color: goldenrod;
  flex-grow: 1;
}
.el--5 {
  background-color: palevioletred;
  align-self: stretch;
  order: 1;
}
.el--6 {
  background-color: steelblue;
  order: -1;
}
.el--7 {
  background-color: yellow;
}
.el--8 {
  background-color: crimson;
}

.container {
  /* STARTER */
  font-family: sans-serif;
  background-color: #ddd;
  font-size: 40px;
  margin: 40px;

  /* FLEXBOX */
  display: flex;
  align-items: center;
  justify-content: center;
  /*justify-content: space-between;*/
}
```
- If you set flex-grow: 1 in all the elements except one and set flex-grow: 2 on that element it will take double of the avalailable empty space (not double the element) than the others. 
- Flex property itself is a short-hand for the previous three properties. flex-grow: 1 is the same as flex: 1. 

### Modify the project using flexbox instead of floats:

- index.html
```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <title>The Basic Language of the Web: HTML</title>
    <link rel="stylesheet" type="text/css" href="styles.css"/>
  </head>

  <body>
    <!--
    <h1>The Basic Language of the Web: HTML</h1>
    <h2>The Basic Language of the Web: HTML</h2>
    <h3>The Basic Language of the Web: HTML</h3>
    <h4>The Basic Language of the Web: HTML</h4>
    <h5>The Basic Language of the Web: HTML</h5>
    <h6>The Basic Language of the Web: HTML</h6>
    -->
    <div class="container">
      <header class="main-header clear-fix">
        <h1>📘 The Code Magazine</h1>
  
        <nav>
          <a href="blog.html">Blog</a>
          <a href="#">Challenges</a>
          <a href="flexbox.html">Flexbox</a>
          <a href="css-grid.html">CSS Grid</a>
        </nav>
      </header>
  
      <article>
        <header class="post-header">
          <h2>The Basic Language of the Web: HTML</h2>
          <div class="author-box"> 
              <img
                src="img/laura-jones.jpg"
                alt="Headshot of Laura Jones"
                class="author-image"
                height="50"
                width="50"
              />
              
              <p id="author" class="author">Posted by <strong>Laura Jones</strong> on 
                Monday, June 21st 2027</p>
            </div>            
          <img
            src="img/post-img.jpg"
            alt="HTML code on a screen"
            width="500"
            height="200"
            class="post-image"
          />
        </header>
  
        <p>
          All modern websites and web applications are built using three
          <em>fundamental</em>
          technologies: HTML, CSS and JavaScript. These are the languages of the
          web.
        </p>
  
        <p>
          In this post, let's focus on HTML. We will learn what HTML is all about,
          and why you too should learn it.
        </p>
  
        <h3>What is HTML?</h3>
        <p>
          HTML stands for <strong>H</strong>yper<strong>T</strong>ext
          <strong>M</strong>arkup <strong>L</strong>anguage. It's a markup
          language that web developers use to structure and describe the content
          of a webpage (not a programming language).
        </p>
        <p>
          HTML consists of elements that describe different types of content:
          paragraphs, links, headings, images, video, etc. Web browsers understand
          HTML and render HTML code as websites.
        </p>
        <p>In HTML, each element is made up of 3 parts:</p>
  
        <ol>
          <li>The opening tag</li>
          <li>The closing tag</li>
          <li>The actual element</li>
        </ol>
  
        <p>
          You can learn more at
          <a
            href="https://developer.mozilla.org/en-US/docs/Web/HTML"
            target="_blank"
            >MDN Web Docs</a
          >.
        </p>
  
        <h3>Why should you learn HTML?</h3>
  
        <p>
          There are countless reasons for learning the fundamental language of the
          web. Here are 5 of them:
        </p>
  
        <ul>
          <li>To be able to use the fundamental web dev language</li>
          <li>
            To hand-craft beautiful websites instead of relying on tools like
            Worpress or Wix
          </li>
          <li>To build web applications</li>
          <li>To impress friends</li>
          <li>To have fun 😃</li>
        </ul>
  
        <p>Hopefully you learned something new here. See you next time!</p>
      </article>
  
      <aside>
        <h4>Related posts</h4>
  
        <ul class="related-posts-list">
          <li class="related-post">
            <img
              src="img/related-1.jpg"
              alt="Person programming"
              width="75"
              width="75"
            />
            <div>
              <a href="#" class="related-link">How to Learn Web Development</a>
              <p class="related-author">By Jonas Schmedtmann</p>
            </div>
          </li>
          <li class="related-post"> 
            <img src="img/related-2.jpg" alt="Lightning" width="75" heigth="75" />
            <div>
              <a href="#" class="related-link">The Unknown Powers of CSS</a>
              <p class="related-author">By Jim Dillon</p>
            </div>
          </li>
          <li class="related-post">
            <img
              src="img/related-3.jpg"
              alt="JavaScript code on a screen"
              width="75"
              height="75"
            />
            <div>
              <a href="#" class="related-link">Why JavaScript is Awesome</a>
              <p class="related-author">By Matilda</p>
            </div>
          </li>
        </ul>
      </aside>
  
      <footer>
        <!--<p id="copyright" class="copyright text">Copyright &copy; 2027 by The Code Magazine.</p>-->
        <p id="copyright">Copyright &copy; 2027 by The Code Magazine.</p>
      </footer>
    </div>
    <button>❤️ Like</button>
  </body>
</html>
```
```css
.main-header{
    display: flex;
    align-items: center;
    justify-content: space-between;
}
.author-box{
    display: flex;
    align-items: center;
    margin-bottom: 15px;;
}

.author{
    margin-bottom: 0;
    margin-left: 15px;
}

.related-post{
    display: flex;
    align-items: center;
    gap: 20px;
    margin-bottom: 20px;
}

.related-author{
    margin-bottom: 0;
    font-size: 14px;
    font-weight: normal;
    font-style: italic;
}

.related-link:link, .related-link:active, .related-link:hover, .related-link:visited{
    font-size: 17px;
    font-weight: bold;
    font-style: normal;
    margin-bottom: 5px;
    display: block;
}
```
### Building a simple flexbox layout

```html
<div class="row">
<article>
  <header class="post-header">
    <h2>The Basic Language of the Web: HTML</h2>
    <div class="author-box"> 
      <img
      src="img/laura-jones.jpg"
      alt="Headshot of Laura Jones"
      class="author-image"
      height="50"
      width="50"
      />
      
      <p id="author" class="author">Posted by <strong>Laura Jones</strong> on 
        Monday, June 21st 2027</p>
      </div>            
      <img
      src="img/post-img.jpg"
      alt="HTML code on a screen"
      width="500"
      height="200"
      class="post-image"
      />
    </header>
    
    <p>
      All modern websites and web applications are built using three
      <em>fundamental</em>
      technologies: HTML, CSS and JavaScript. These are the languages of the
      web.
    </p>
    
    <p>
      In this post, let's focus on HTML. We will learn what HTML is all about,
      and why you too should learn it.
    </p>
    
    <h3>What is HTML?</h3>
    <p>
      HTML stands for <strong>H</strong>yper<strong>T</strong>ext
      <strong>M</strong>arkup <strong>L</strong>anguage. It's a markup
      language that web developers use to structure and describe the content
      of a webpage (not a programming language).
    </p>
    <p>
      HTML consists of elements that describe different types of content:
      paragraphs, links, headings, images, video, etc. Web browsers understand
      HTML and render HTML code as websites.
    </p>
    <p>In HTML, each element is made up of 3 parts:</p>
    
    <ol>
      <li>The opening tag</li>
      <li>The closing tag</li>
      <li>The actual element</li>
    </ol>
    
    <p>
      You can learn more at
      <a
      href="https://developer.mozilla.org/en-US/docs/Web/HTML"
      target="_blank"
      >MDN Web Docs</a
      >.
    </p>
    
    <h3>Why should you learn HTML?</h3>
    
    <p>
      There are countless reasons for learning the fundamental language of the
      web. Here are 5 of them:
    </p>
    
    <ul>
      <li>To be able to use the fundamental web dev language</li>
      <li>
        To hand-craft beautiful websites instead of relying on tools like
        Worpress or Wix
      </li>
      <li>To build web applications</li>
      <li>To impress friends</li>
      <li>To have fun 😃</li>
    </ul>
    
    <p>Hopefully you learned something new here. See you next time!</p>
  </article>
  
  <aside>
    <h4>Related posts</h4>
    
    <ul class="related-posts-list">
      <li class="related-post">
        <img
        src="img/related-1.jpg"
        alt="Person programming"
        width="75"
        width="75"
        />
        <div>
          <a href="#" class="related-link">How to Learn Web Development</a>
          <p class="related-author">By Jonas Schmedtmann</p>
        </div>
      </li>
      <li class="related-post"> 
        <img src="img/related-2.jpg" alt="Lightning" width="75" heigth="75" />
        <div>
          <a href="#" class="related-link">The Unknown Powers of CSS</a>
          <p class="related-author">By Jim Dillon</p>
        </div>
      </li>
      <li class="related-post">
        <img
        src="img/related-3.jpg"
        alt="JavaScript code on a screen"
        width="75"
        height="75"
        />
        <div>
          <a href="#" class="related-link">Why JavaScript is Awesome</a>
          <p class="related-author">By Matilda</p>
        </div>
      </li>
    </ul>
  </aside>
</div>
```
```css
.row{
    display: flex;
    gap: 75px;
    margin-bottom: 60px;
    align-items: flex-start;
}

article{
    flex: 1;
    margin-bottom: 0;
}

aside{
    flex: 0 0 300px;
}
```
### Introduction to CSS Grid

- Display: grid
- Two dimensional layout: columns and rows.
- To create two columns, one of 250px and another of 150px:
```css
  .container--1 {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 40px;
    margin: 40px;

    /* CSS GRID */
    display: grid;
    grid-template-columns: 250px 150px;
  }
```
- The following code creates a 4 columns by two rows with the elements inside the container:
```html
 <div class="container--1">
  <div class="el el--1">(1) HTML</div>
  <div class="el el--2">(2) and</div>
  <div class="el el--3">(3) CSS</div>
  <div class="el el--4">(4) are</div>
  <div class="el el--5">(5) amazing</div>
  <div class="el el--6">(6) languages</div>
  <div class="el el--7">(7) to</div>
  <div class="el el--8">(8) learn</div>
</div>
```
```css
  .container--1 {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 32px;
    margin: 40px;

    /* CSS GRID */
    display: grid;
    grid-template-columns: 200px 200px 100px 100px;
  }
```

|(1) HTML   | (2) and   | (3) CSS   | (4) are  |
|---|---|---|---|
| (5) amazing  | (6) languages   | (7) to   | (8) learn   |
- The entire row got the height of the element (grid item) with the highest value (height property). 
- We can also size the row:
```css
.container{
  grid-template-rows: 300px 200px;
}
```
- You can also specify gap to set space between grid items. You can set different gap for the column and for the row.
```css
.container--1 {
    /* STARTER */
    font-family: sans-serif;
    background-color: #ddd;
    font-size: 32px;
    margin: 40px;

    /* CSS GRID */
    display: grid;
    grid-template-columns: 200px 200px 100px 100px;
    grid-template-rows: 300px 200px;
    /*gap: 20px;*/
    column-gap: 20px;
    row-gap: 60px;
  }
```
### CSS grid overview

- Set CSS properties to build 2 dimensional layouts.
- Divide a container element into rows and columns that can be filled with its child elements. 
- Less nested html and easier to read CSS.
- Is not meant to replace flexbox. They work together. Need a 1D layout? Flexbox. Need a 2D layout? Use CSS Grid.
- Grid container.
  - grid-template-rows: <track-size>
  - grid-template-columns: <track-size>
  - row-gap: **0** | length
  - column-gap: **0** | length
  - justify-items: **stretch** | start | center | end
  - align-items: **stretch** | start | center | end
  - justify-content: **stretch** | start | center | end

- Grid items. 
  - grid-column: start line | end line
  - grid-row: start line | end line
  - justify-self: **stretch** | start | center | end
  - align-self: **stretch** | start | center | end
- Row axis
- Column axis. We cannot change direction of axis. 
- Grid lines that divide the columns and the rows. They are numbered. Those numbers are important because we can place a grid item in a specific place using that number.
- Grid cells might be filled by a grid item or not.
- Gutter or gaps (spaces).
- Grid track/row (can be a row or a column). Two row tracks and two column tracks.

### Sizing grid columns and rows

- 1 fr (fraction) will take all the remaining available space to grow. If you set more than one element with 1 fr they will share the available space. 
- If you use 2fr and the other 1fr the 2fr will take double the space than the 1fr.
- You can also use auto property. This means the column will take only the necessary space to fill its content. 
```css
.container--1 {
  /* STARTER */
  font-family: sans-serif;
  background-color: #ddd;
  font-size: 32px;
  margin: 40px;

  /* CSS GRID */
  display: grid;
  grid-template-columns: 2fr 1fr 1fr auto;
  grid-template-rows: 300px 200px;
  column-gap: 10px;
  row-gap: 40px;
  }
```
- repeat property (how many columns you want and its size)
- repeat (4, 1fr) //same as 1fr 1fr 1fr 1fr
- using 1fr for the grid-template-rows will take the height of the highest element. If you dont define a height for the container it will take the height required to display its content.

### Placing and spacing elements

- You can verify the column and row numbers in grid section in dev tools.
- It is set at grid item level. To move element-8 in first row and between 2 and 3 columns:
```css
 .el--8 {
    background-color: crimson;
    grid-column: 2 / 3; /*start at column 2 and end in column 3*/
    grid-row: 1 / 2;
  }
```
- We can omit the number after / if it is +2
- You can also use span keyword to specify how many columns do you want it to take:
```css
.el--8 {
    background-color: crimson;
    grid-column: 2 / span 3; /*start at column 2 and end in column 3*/
    grid-row: 1 / 2;
  }
```
- You can use -1 to span to the end:
```css
.el--8 {
  background-color: crimson;
  grid-column: 2 / -1; /*start at column 2 and end in column 3*/
  grid-row: 1 / 2;
}
```
### Aligning grid items and tracks

- Align the grid tracks inside of the grid container (distributing empty space):
- content properties align tracks inside of the container. justify is horizontally on the row axis and align is vertically.
```css
.container--2 {
  /* STARTER */
  font-family: sans-serif;
  background-color: black;
  font-size: 40px;
  margin: 40px;

  width: 1000px;
  height: 600px;

  /* CSS GRID */
  display: grid;
  grid-template-columns: 125px 200px 125px;
  grid-template-rows: 250px 100px;
  gap: 50px;

  justify-content: center;
  /* justify-content:  space-between; */
  align-content: center;

}
```
- Align items inside cells, moving items around inside the cells:

```css
  .container--2 {
    /* STARTER */
    font-family: sans-serif;
    background-color: black;
    font-size: 40px;
    margin: 40px;

    width: 1000px;
    height: 600px;

    /* CSS GRID */
    display: grid;
    grid-template-columns: 125px 200px 125px;
    grid-template-rows: 250px 100px;
    gap: 50px;

    justify-content: center;
    /* justify-content:  space-between; */
    align-content: center;

    align-items: center;
    justify-items: center;
  }
```
- justify-items and align-items by default are set to stretch.
- You can override them individually (align-self):
```css
.container--2 {
    /* STARTER */
    font-family: sans-serif;
    background-color: black;
    font-size: 40px;
    margin: 40px;

    width: 1000px;
    height: 600px;

    /* CSS GRID */
    display: grid;
    grid-template-columns: 125px 200px 125px;
    grid-template-rows: 250px 100px;
    gap: 50px;

    justify-content: center;
    /* justify-content:  space-between; */
    align-content: center;

    align-items: center;
    justify-items: center;
  }

  .el--1 {
    background-color: blueviolet;
    align-self: end;
  }
```
