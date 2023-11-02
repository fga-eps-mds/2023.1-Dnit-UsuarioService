#!/bin/bash

set -x
WORKDIR=$(pwd)

python -m venv venv
source /opt/venv/bin/activate

pip install -r "${WORKDIR}/requirements.txt"

cat > /etc/systemd/system/dnit-deploy.service <<EOF
[Unit]
Description = Deploy Dnit Service

[Service]
WorkingDirectory=$WORKDIR
ExecStart=$WORKDIR/venv/bin/python -m uvicorn main:app --host 0.0.0.0 --port 30001
Restart=on-failure

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable dnit-deploy.service --now
