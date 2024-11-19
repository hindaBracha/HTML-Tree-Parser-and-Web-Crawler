# Web Crawler and HTML Tree Parser in C#

This project is a simple web crawler implemented in C# that loads an HTML page from a given URL, parses it into a tree structure, and allows for searching the tree using a custom CSS-like selector query. The crawler constructs a hierarchical representation of the HTML structure and searches for matching elements, printing the results to the console.

## Features

- Fetches HTML content from a URL using `HttpClient`.
- Parses the HTML content into a tree structure of HTML elements.
- Supports searching the HTML tree with custom CSS-like selectors.
- Outputs the matching HTML elements based on the search query.

## Structure

The project is divided into the following main components:

- **Program.cs**: The entry point of the application. This file handles loading the HTML, parsing it into a tree structure, and searching the tree using a selector.
- **HtmlElement.cs**: Defines the `HtmlElement` class, which represents an HTML element in the parsed tree. It includes methods for traversing descendants, ancestors, and searching for elements that match a selector.
- **HtmlHelper.cs**: A utility class that loads predefined HTML tags and void tags from JSON files, making it easier to parse the HTML structure correctly.
- **Selector.cs**: Contains the `selector` class, which represents a CSS-like selector query. It is used to match elements in the HTML tree based on tag name, class, and ID.

## How It Works

1. **Loading HTML**:
   - The `Load` method fetches the HTML content from the provided URL using `HttpClient`. The raw HTML string is then cleaned by removing unnecessary whitespace.

2. **Building the HTML Tree**:
   - The `buildTree` method parses the HTML content into a tree of `HtmlElement` objects. Each element is represented by its tag name, attributes (such as `id` and `class`), and any child elements.

3. **Searching the Tree**:
   - The `search` method allows you to search the HTML tree using a custom CSS-like selector. You can search by tag name, class, or ID.
   - The `selector` class interprets the query string and converts it into a selector tree. The search is performed recursively, returning all matching elements.

4. **Output**:
   - After searching the tree for elements that match the query, the matching elements are printed to the console.

## Example Usage

1. Clone the repository and open the project in Visual Studio or any other C# IDE.
2. Run the program by executing `Program.cs`.
3. The program will fetch the HTML from the given URL, parse it into a tree, and search for elements based on the specified selector.

### Example Query:
```csharp
selector selector = selector.parsQuery("body .class1 #id");
