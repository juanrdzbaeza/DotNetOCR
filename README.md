# DotNetOCR

DotNetOCR is a .NET application that uses Tesseract OCR engine to perform optical character recognition on images.

## Prerequisites

- .NET Framework (version used in your project)
- Tesseract OCR engine
- Tesseract language data files

## Setup

1. Clone this repository or download the source code.
2. Ensure you have the required Tesseract language data files (e.g., `spa.traineddata` for Spanish).

### Setting up Tesseract Data Files

You have two options to set up the Tesseract data files:

#### Option 1: Using TESSDATA_PREFIX Environment Variable

1. Create a directory to store your Tesseract data files (e.g., `C:\tessdata`).
2. Place your language data files (e.g., `spa.traineddata`) in this directory.
3. Set the TESSDATA_PREFIX environment variable:
   - Open System Properties (Right-click on This PC > Properties)
   - Click on Advanced system settings
   - Click on Environment Variables
   - Under System variables, click New
   - Set Variable name as TESSDATA_PREFIX
   - Set Variable value as the path to your tessdata directory (e.g., C:\tessdata\)
   - Click OK to save

#### Option 2: Specifying the Path in Code

If you prefer not to use an environment variable, you can specify the path to the tessdata directory directly in your code:

1. Create a directory to store your Tesseract data files (e.g., `C:\tessdata`).
2. Place your language data files (e.g., `spa.traineddata`) in this directory.
3. In your code, when initializing the TesseractEngine, provide the path to the tessdata directory:

```csharp
string tessdataPath = @"C:\tessdata\";
using (var engine = new TesseractEngine(tessdataPath, "spa", EngineMode.Default))
{
    // Rest of your OCR code
}
```


## Usage
1. Run the application.
2. Click the "Select Image" button to choose an image for OCR processing.
3. The recognized text will be displayed in the text area.

### Troubleshooting

If you encounter the error "Failed to initialise tesseract engine", ensure that:
- You have downloaded the correct language data files.
- The TESSDATA_PREFIX environment variable is set correctly (if using Option 1).
- The path to the tessdata directory is correct in your code (if using Option 2).
- The language code used in the TesseractEngine constructor matches your language data file (e.g., "spa" for Spanish).

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE.txt file for details.