<div align="center">
  <br />
  <h1>Desktop Note Manager</h1>
  <p>
    A lightweight, high-performance note-taking application designed for speed and simplicity.
    <br />
    Developed with C# and WPF.
  </p>
</div>

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#technical-architecture">Technical Architecture</a></li>
    <li><a href="#key-features">Key Features</a></li>
    <li><a href="#installation">Installation</a></li>
    <li><a href="#usage">Usage</a></li>
  </ol>
</details>

<h2 id="about-the-project">About The Project</h2>

<p>
  This application originated as a proprietary tool for personal workflow management and has been refactored for public release. It addresses the need for a distraction-free environment to manage text-based data efficiently.
</p>
<p>
  Unlike complex note-taking suites that consume significant system resources, this project focuses on minimalism and immediacy. The user interface eliminates visual clutter, allowing users to focus entirely on content creation and retrieval.
</p>

<h2 id="technical-architecture">Technical Architecture</h2>

<p>The application is built using the Microsoft .NET ecosystem, leveraging the following technologies for stability and performance:</p>

<ul>
  <li><strong>Language:</strong> C#</li>
  <li><strong>Framework:</strong> Windows Presentation Foundation (WPF)</li>
  <li><strong>Frontend:</strong> XAML (Extensible Application Markup Language)</li>
</ul>

<h2 id="key-features">Key Features</h2>

<h3>Automated Persistence (Auto-Save)</h3>
<p>
  To prevent data loss without interrupting the user workflow, the application features an intelligent auto-save mechanism. The system monitors user input and automatically commits changes to storage after <strong>2 seconds of inactivity</strong>. This ensures that the state is preserved even in the event of an unexpected shutdown.
</p>

<h3>High-Performance Search</h3>
<p>
  Designed to handle a large volume of text files, the application includes a dedicated search bar. It utilizes optimized string matching to filter through documents instantly, allowing for rapid retrieval of specific notes regardless of the library size.
</p>

<h3>Streamlined UI/UX</h3>
<p>
  <ul>
    <li><strong>Minimalist Design:</strong> A clean interface that adheres to modern design principles, reducing cognitive load.</li>
    <li><strong>Rapid IO Operations:</strong> Optimized specifically for fast reading, writing, and deletion of text documents.</li>
  </ul>
</p>

<h2 id="installation">Installation</h2>

<p>This project includes a pre-compiled installer package for ease of deployment.</p>

<ol>
  <li>Navigate to the <strong>Releases</strong> section of this repository.</li>
  <li>Download the latest <code>.msi</code> or <code>.exe</code> installer package.</li>
  <li>Run the installer and follow the on-screen wizard instructions to deploy the application to your local machine.</li>
</ol>

<h2 id="usage">Usage</h2>

<p>Upon launching the application, users are presented with the main dashboard. New notes can be created immediately. Existing notes are listed and can be filtered using the search bar at the top of the window. The auto-save function is active by default and requires no configuration.</p>
