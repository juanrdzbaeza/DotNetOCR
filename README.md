# DotNetOCR

Aplicación Windows Forms en .NET 10 que usa Tesseract para realizar OCR sobre PDFs e imágenes.

## Características

- Procesado de PDFs por página (renderizado con PdfiumViewer).
- Procesado de imágenes (PNG/JPEG).
- Soporta selección de archivos y pegar desde el portapapeles.
- Integración con Tesseract (soporta `spa`/`eng` según los datos instalados).

## Requisitos

- .NET 10 (Windows)
- Tesseract (librería nativa y archivos de datos `tessdata`)
- Paquetes NuGet usados:
  - `Tesseract` (v5.x)
  - `PdfiumViewer`

El proyecto está configurado para `x64` en `PlatformTarget`.

## Instalación y ejecución

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

Nota: el proyecto es una aplicación WinForms; se recomienda abrirla con Visual Studio para una experiencia completa.

## Configurar los datos de Tesseract (`tessdata`)

Tesseract necesita los archivos `.traineddata` para funcionar (por ejemplo `spa.traineddata`). Hay dos opciones:

- Opción A — Variable de entorno `TESSDATA_PREFIX` (recomendado):
  1. Crea una carpeta con los archivos de lenguaje (ej. `C:\tessdata`).
  2. Añade la variable de entorno `TESSDATA_PREFIX` apuntando a esa carpeta.

- Opción B — Carpeta `tessdata` junto al ejecutable:
  Coloca una carpeta `tessdata` en el mismo directorio que el exe con los `.traineddata` dentro.

En la UI la aplicación intenta usar `TESSDATA_PREFIX`, si no existe busca `tessdata` en el directorio de la app y, si no lo encuentra, solicita que el usuario seleccione la carpeta.

## Uso

1. Inicia la aplicación.
2. Haz clic en `Seleccionar PDF/Imagen` para elegir un archivo o en `Pegar` para procesar la imagen del portapapeles.
3. El texto resultante aparecerá en el área de salida con confianza por página (para PDFs).

## Troubleshooting — problemas comunes

- "Failed to initialise tesseract engine":
  - Asegúrate de que los archivos `*.traineddata` estén en la carpeta `tessdata` correcta y que el idioma solicitado exista (por ejemplo `spa.traineddata`).
  - Verifica la variable de entorno `TESSDATA_PREFIX` si la usas.

- `MissingManifestResourceException` al cargar iconos o recursos del formulario:
  - La forma en que WinForms carga recursos del diseñador espera que el `.resx` del formulario se compile como `EmbeddedResource`.
  - Si ves una excepción del tipo "Could not find the resource 'DotNetOCR.DotNetOCR.resources'", abre `DotNetOCR.csproj` y asegura que `DotNetOCR.resx` está incluido como `EmbeddedResource` (no marcado como `None` o removido). Ejemplo:

    ```xml
    <ItemGroup>
      <EmbeddedResource Include="DotNetOCR.resx" />
    </ItemGroup>
    ```

  - Alternativamente puedes cargar el icono desde archivo en tiempo de ejecución:

    ```csharp
    this.Icon = new System.Drawing.Icon(Path.Combine(AppContext.BaseDirectory, "Resources", "OCR_icon.ico"));
    ```

- PDF rendering / native `pdfium.dll`:
  - El proyecto contiene un `Target` que intenta copiar `native\$(PlatformTarget)\pdfium.dll` al directorio de salida si existe. Asegúrate de proporcionar esa DLL nativa para render de PDFs si la necesitas.

## Estructura relevante

- `DotNetOCR/` — proyecto principal (WinForms).
- `DotNetOCR/DotNetOCR.resx` — recursos del formulario (icono embebido).
- `Resources/` — iconos e imágenes incluidos como `None` en el proyecto; el icono de la aplicación puede configurarse en la propiedad `ApplicationIcon`.

## Contribuir

1. Haz fork y crea una rama con tu cambio (`feature/mi-cambio`).
2. Abre un Pull Request describiendo los cambios.

Buenas prácticas:
- Mantener los archivos `.resx` del diseñador como `EmbeddedResource`.
- No incluir blobs binarios enormes en los archivos de texto; preferir recursos binarios separados o la carpeta `Resources/`.

## Licencia

Proyecto bajo la licencia MIT — ver `LICENSE.txt`.
