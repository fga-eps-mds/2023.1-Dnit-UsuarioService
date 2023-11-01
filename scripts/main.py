from fastapi import FastAPI
from subprocess import call
import tarfile
import os
import shutil

STAGE_DIR = "/home/frostwagner/unb/EPS/api_updater/stage"
TARGET_DIR = "/home/frostwagner/unb/EPS/api_updater/target"
APPSETTINGS_FILE = "/home/frostwagner/unb/EPS/api_updater/appsettings.json"
SYSTEMD_START_FILE = "/home/frostwagner/unb/EPS/api_updater/start.sh"
SYSTEMD_SERVICE = "sshd"

app = FastAPI()

@app.post("/update/{build_name}")
def update_deploy(build_name: str):
    TAR_FILE = f'{STAGE_DIR}/{build_name}'
    # download build to STAGE_DIR

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


