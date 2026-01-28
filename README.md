# SmartFactory Digital Twin Viewer

## Overview

Unity 기반 디지털 트윈 환경에서 MQTT로 수신한 배달 로봇의 상태 및 위치 데이터를 3D 공간과 UI 대시보드에 실시간으로 시각화하는 관제 시스템

## Architecture

- Unity (Client)
- AWS IoT Core (MQTT)
- AWS Lambda + API Gateway

## Core Features

- 실시간 센서 데이터 수신
- 상태 기반 3D 시각화
- 장비 상태 모니터링 UI

## Data Flow

RobotSensor → MQTT → Unity → Model → View
