from typing import Union
from typing_extensions import Annotated
from fastapi import FastAPI, UploadFile, Header
from subprocess import call
import tarfile
import os
import shutil

from fastapi.responses import JSONResponse

STAGE_DIR = "stage"
TARGET_DIR = "target"
APPSETTINGS_FILE = os.getenv("APPSETTINGS_FILE","appsettings.json")
SYSTEMD_SERVICE =  os.getenv("SYSTEMD_SERVICE","usuarioservice")

SECRET = os.getenv("SECRET_KEY", "secret")

def prepare():
    os.makedirs(STAGE_DIR, exist_ok=True)
    os.makedirs(TARGET_DIR, exist_ok=True)
    if not os.path.exists(APPSETTINGS_FILE):
        raise FileNotFoundError(f"File {APPSETTINGS_FILE} not found")

prepare()

app = FastAPI()

@app.post("/update/{build_name}")
async def update_deploy(build_name: str, file: UploadFile, upload_token: Annotated[str, Header()]):
    if upload_token != SECRET:
        return JSONResponse(content={"error": "invalid token"}, status_code=401)

    prepare()

    # save file to STAGE_DIR
    TAR_FILE = f'{STAGE_DIR}/{build_name}'
    with open(TAR_FILE, "wb") as fd:
        fd.write(file.file.read())

    # untar it
    with tarfile.open(TAR_FILE) as tar:
        tar.extractall(path=STAGE_DIR)

    os.remove(TAR_FILE)

    next_build_id = len(os.listdir(TARGET_DIR))+1
    shutil.move(f"{STAGE_DIR}", f"{TARGET_DIR}/{next_build_id}")

    # set appsettings.json
    SETTINGS_FILE = f"{TARGET_DIR}/{next_build_id}/build/appsettings.json"
    if os.path.exists(SETTINGS_FILE):
        os.remove(SETTINGS_FILE)
    os.symlink(APPSETTINGS_FILE, SETTINGS_FILE)

    # restart systemd service
    systemd_restart_status = call(["systemctl", "restart", SYSTEMD_SERVICE])
    if systemd_restart_status:
        return JSONResponse(content={"error": f"Error at restarting {SYSTEMD_SERVICE}"}, status_code=500)

    return {"status": "success"}
