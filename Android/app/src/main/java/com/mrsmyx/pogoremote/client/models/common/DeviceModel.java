package com.mrsmyx.pogoremote.client.models.common;

/**
 * Created by Charlton on 7/27/2016.
 */
public class DeviceModel {

    private String device;
    private String version;
    private String model;
    private String ip;

    public static DeviceModel Factory() {
        return new DeviceModel();
    }

    private DeviceModel() {
    }

    public DeviceModel setDevice(String device) {
        this.device = device;
        return this;
    }

    public DeviceModel setVersion(String version) {
        this.version = version;
        return this;
    }

    public DeviceModel setModel(String model) {
        this.model = model;
        return this;
    }

    public DeviceModel setIp(String ip) {
        this.ip = ip;
        return this;
    }
}
