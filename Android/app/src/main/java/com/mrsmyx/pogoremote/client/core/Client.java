package com.mrsmyx.pogoremote.client.core;

import com.mrsmyx.pogoremote.client.models.ResponseModel;

/**
 * Created by Charlton on 7/27/2016.
 */
public class Client {
    private int port;

    protected Client(int port){
        this.port = port;
    }

    public <T> void sendData(ResponseModel<T> responseModel){
        
    }


}
