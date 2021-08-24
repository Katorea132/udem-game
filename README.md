# udem-game
Este es el repositorio tanto para el front (SoonTM) como para el back  del juego para la Universidad de Medellín.

## Requisitos

Tener instaladas en el computador las siguientes herramientas
* Python 3.8+
* MYSQL server o base de datos de preferencia (si no es MYSQL/mariadb, el código debe ser cambiado acorde)
* Inicializar el servidor de base de datos y crear una base de datos donde alojarla, este nombre debe ir en el config.json
* Idealmente una herramienta para [entornos virtuales](https://docs.python.org/3/library/venv.html)
* Un archivo config.json en la carpeta back-end (el database_engine y database_driver deben ser cambiados dependiendo de la base de datos utilizada)
```javascript
{
    "username": "database username",
    "password": "database username's password",
    "server": "database server location",
    "database_engine": "mysql",
    "database_driver": "pymysql",
    "database_name": "nombre asignado"
}
```
## Uso

Para iniciar, clona el repositorio en el lugar deseado para alojarlo, navega a la carpeta udem-game y posteriormente a la carpeta back-end. Una vez allí:
* (Opcional) Crea un entorno virtual para evitar conflictos de versiones con otros proyectos locales
* Instala las dependencias listadas en requirements.txt
* Inicia el main.py
```bash
# Se asume Linux
git clone https://github.com/Katorea132/udem-game.git
cd ./udem-game/back-end/
python3 -m venv $nombredeseado
source $nombredeseado/bin/activate
pip install -r requirements.txt
python3 main.py
```
Con esto deberías tener ya el API corriendo y escuchando peticiones.
