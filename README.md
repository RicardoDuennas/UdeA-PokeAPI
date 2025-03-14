# UdeA-PokeAPI
## Prueba técnica para el cargo de Desarrollador Unity en la Universidad de Antioquia

![Screenshot of the 3D environmet](/Assets/Images/UdeA-PokeAPI.png)

Se desarrolló un escenario 3D con el objetivo de capturar Pokébolas para hacer crecer un inventario que se muestra por medio de una lista y tarjetas para cada Pokémon capturado.

Siguiendo las indicaciones de la prueba, se desarrollaron los siguientes sistemas:

- Controlador de personaje por medio de las teclas W, S (movimiento adelante y atrás) y el mouse (dirección).
- Creación y ambientación de un entorno 3D de estilo natural usando Assets gráficos y de audio de uso libre.
- Implementación del código para el consumo de la API pokeapi.co, seleccionando la información de 20 Pokémones al azar.
- Implementación de Object Pool para adiministrar las Pokébolas en la escena.
- Implementación de un sistema de inventarios para las Pokébolas capturadas, guardando la siguiente información de la API: id, nombre, tipo, habilidades y movimientos de cada Pokémon.
- Implementación de un sistema de IU por medio de UI Toolkit que muestra: el listado de Pokémones capturados, las características de cada Pokémon seleccionado en formato de tarjeta, mensajes informativos y un panel de bienvenida. Mientras se muestra el panel lateral, el juego entra en pausa.
- Implementación de un sistema de persistencia (guardado y carga del estado del juego) por medio de serialización en un archivo JSON.
- Selección de juego nuevo o carga del archivo encontrado.

## Lista de Assets usados en el proyecto:
- Ambiente 3D | https://assetstore.unity.com/packages/3d/environments/free-stylized-nature-environment-96371
- Imagen del jugador | https://unsplash.com/es/fotos/una-imagen-borrosa-de-un-edificio-en-la-oscuridad-kBzQNk9AgOg
- Imagen del agua | https://unsplash.com/es/fotos/un-fondo-azul-borroso-con-lineas-horizontales-NZ0HxSy55hY
- Imagen de fondo de la tarjeta | https://unsplash.com/es/fotos/papel-blanco-de-impresora-sobre-mesa-de-madera-marron-794QUz5-cso
- Audio general del juego | Rhythmic Game Menu Ambience by PatrickLieberkind https://freesound.org/s/396024/ License: Attribution 4.0
- Efecto de audio de captura de Pokémones | UI Confirmation Alert, C4.wav by InspectorJ https://freesound.org/s/403019/ License: Attribution 4.0

## Nota
El build para Windows se encuentra en la carpeta WinBuild

Ricardo Alfonzo Dueñas - 2025
