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