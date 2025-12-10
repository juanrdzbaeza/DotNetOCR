# DotNetOCR

Aplicaci�n Windows Forms en .NET 10 que usa Tesseract para realizar OCR sobre PDFs e im�genes.

## Caracter�sticas

- Procesado de PDFs por p�gina (renderizado con PdfiumViewer).
- Procesado de im�genes (PNG/JPEG).
- Soporta selecci�n de archivos y pegar desde el portapapeles.
- Integraci�n con Tesseract (soporta `spa`/`eng` seg�n los datos instalados).

## Requisitos

- .NET 10 (Windows)
- Tesseract (librer�a nativa y archivos de datos `tessdata`)
- Paquetes NuGet usados:
  - `Tesseract` (v5.x)
  - `PdfiumViewer`

El proyecto est� configurado para `x64` en `PlatformTarget`.

## Instalaci�n y ejecuci�n

1. Clona el repositorio:

   ```bash
   git clone https://github.com/juanrdzbaeza/DotNetOCR.git
   cd DotNetOCR/DotNetOCR
   ```

2. Restaurar paquetes y construir:

   ```bash
   dotnet restore
   dotnet build -c Release
   ```

3. Ejecutar (desde Visual Studio o CLI):

   ```bash
   dotnet run --project DotNetOCR -c Release
   ```

Nota: el proyecto es una aplicaci�n WinForms; se recomienda abrirla con Visual Studio para una experiencia completa.

## Configurar los datos de Tesseract (`tessdata`)

Tesseract necesita los archivos `.traineddata` para funcionar (por ejemplo `spa.traineddata`). Hay dos opciones:

- Opci�n A � Variable de entorno `TESSDATA_PREFIX` (recomendado):
  1. Crea una carpeta con los archivos de lenguaje (ej. `C:\tessdata`).
  2. A�ade la variable de entorno `TESSDATA_PREFIX` apuntando a esa carpeta.

- Opci�n B � Carpeta `tessdata` junto al ejecutable:
  Coloca una carpeta `tessdata` en el mismo directorio que el exe con los `.traineddata` dentro.

En la UI la aplicaci�n intenta usar `TESSDATA_PREFIX`, si no existe busca `tessdata` en el directorio de la app y, si no lo encuentra, solicita que el usuario seleccione la carpeta.

## Uso

1. Inicia la aplicaci�n.
2. Haz clic en `Seleccionar PDF/Imagen` para elegir un archivo o en `Pegar` para procesar la imagen del portapapeles.
3. El texto resultante aparecer� en el �rea de salida con confianza por p�gina (para PDFs).

## Troubleshooting � problemas comunes

- "Failed to initialise tesseract engine":
  - Aseg�rate de que los archivos `*.traineddata` est�n en la carpeta `tessdata` correcta y que el idioma solicitado exista (por ejemplo `spa.traineddata`).
  - Verifica la variable de entorno `TESSDATA_PREFIX` si la usas.

- `MissingManifestResourceException` al cargar iconos o recursos del formulario:
  - La forma en que WinForms carga recursos del dise�ador espera que el `.resx` del formulario se compile como `EmbeddedResource`.
  - Si ves una excepci�n del tipo "Could not find the resource 'DotNetOCR.DotNetOCR.resources'", abre `DotNetOCR.csproj` y asegura que `DotNetOCR.resx` est� incluido como `EmbeddedResource` (no marcado como `None` o removido). Ejemplo:

    ```xml
    <ItemGroup>
      <EmbeddedResource Include="DotNetOCR.resx" />
    </ItemGroup>
    ```

  - Alternativamente puedes cargar el icono desde archivo en tiempo de ejecuci�n:

    ```csharp
    this.Icon = new System.Drawing.Icon(Path.Combine(AppContext.BaseDirectory, "Resources", "OCR_icon.ico"));
    ```

- PDF rendering / native `pdfium.dll`:
  - El proyecto contiene un `Target` que intenta copiar `native\$(PlatformTarget)\pdfium.dll` al directorio de salida si existe. Aseg�rate de proporcionar esa DLL nativa para render de PDFs si la necesitas.

## Estructura relevante

- `DotNetOCR/` � proyecto principal (WinForms).
- `DotNetOCR/DotNetOCR.resx` � recursos del formulario (icono embebido).
- `Resources/` � iconos e im�genes incluidos como `None` en el proyecto; el icono de la aplicaci�n puede configurarse en la propiedad `ApplicationIcon`.

## Contribuir

1. Haz fork y crea una rama con tu cambio (`feature/mi-cambio`).
2. Abre un Pull Request describiendo los cambios.

Buenas pr�cticas:
- Mantener los archivos `.resx` del dise�ador como `EmbeddedResource`.
- No incluir blobs binarios enormes en los archivos de texto; preferir recursos binarios separados o la carpeta `Resources/`.

## Licencia

Proyecto bajo la licencia MIT � ver `LICENSE.txt`.
