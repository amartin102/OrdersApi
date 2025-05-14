# OrdersApi (.NET + Docker)

Este proyecto es una aplicación ASP.NET Core que se ejecuta en un contenedor Docker basado en Linux.

## 📦 Requisitos

- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (con contenedores Linux habilitados)

## 🚀 Pasos para compilar y ejecutar en Docker

### 1. Publicar el proyecto para Linux

```bash
dotnet publish -c Release -r linux-x64 --self-contained false -o publish
```

### 2. Construir la imagen Docker

```bash
docker build -t ordersapi-image .
```

### 3. Ejecutar el contenedor

```bash
docker run -d -p 8183:8183 --name ordersapi-container ordersapi-image
```

### 4. Acceder a la API
Una vez el contenedor esté corriendo, abre tu navegador y visita:

```bash
http://localhost:8183/swagger/index.html
```


## 📁 Estructura esperada

```
/OrdersApi/
├── publish/
│   └── (archivos publicados con dotnet publish)
├── Dockerfile
├── README.md
```

---

## 🧰 Requisitos

- [.NET SDK 6 o superior](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)

---

## ✅ Notas

- Asegúrate de tener Docker configurado en modo contenedores Linux para evitar errores de compatibilidad.
- Si ya tienes contenedores en modo Windows, tendrás que detenerlos al cambiar al modo Linux, ya que Docker Desktop no ejecuta ambos al mismo tiempo.