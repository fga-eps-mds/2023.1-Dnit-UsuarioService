from fastapi import FastAPI, UploadFile
from subprocess import call
import tarfile
import os
import shutil

STAGE_DIR = "stage"
TARGET_DIR = "target"
APPSETTINGS_FILE = "appsettings.json"
SYSTEMD_START_FILE = "./start.sh"
SYSTEMD_SERVICE = "sshd"

def prepare():
    os.makedirs(STAGE_DIR, exist_ok=True)
    os.makedirs(TARGET_DIR, exist_ok=True)
    if not os.path.exists(APPSETTINGS_FILE):
        raise FileNotFoundError(f"File {APPSETTINGS_FILE} not found")
    systemd_start_file_dir = os.path.dirname(SYSTEMD_START_FILE)
    if not os.path.exists(systemd_start_file_dir):
        os.makedirs(systemd_start_file_dir, exist_ok=True)

prepare()

app = FastAPI()

@app.post("/update/{build_name}")
def update_deploy(build_name: str, file: UploadFile):
    # save file to STAGE_DIR
    TAR_FILE = f'{STAGE_DIR}/{build_name}'
    with open(TAR_FILE, "wb") as fd:
        fd.write(file.file.read())

    # untar it
    with tarfile.open(TAR_FILE) as tar:
        tar.extractall(path=STAGE_DIR)

    next_build_id = len(os.listdir(TARGET_DIR))+1
    shutil.move(f"{STAGE_DIR}/publish", f"{TARGET_DIR}/{next_build_id}")
    os.remove(TAR_FILE)

    # set appsettings.json
    shutil.copy(APPSETTINGS_FILE, f"{TARGET_DIR}/{next_build_id}/appsettings.json")

    # change context in file to systemd
    with open(SYSTEMD_START_FILE, "w") as fd:
        fd.write(f"#!/bin/bash\n\ndotnet {TARGET_DIR}/{next_build_id}/app.dll")

    # restart systemd service
    systemd_restart_status = call(["systemctl", "restart", SYSTEMD_SERVICE])
    if systemd_restart_status == 0:
        print("ok")
    else:
        print(f"Error at restarting {SYSTEMD_SERVICE}")

