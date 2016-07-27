package com.mrsmyx.pogoremote.client.models;

/**
 * Created by Charlton on 7/27/2016.
 */
public class ResponseModel<T> {
    private int status;
    private String message;
    private T data;

    public int getStatus() {
        return status;
    }

    public String getMessage() {
        return message;
    }

    public T getData() {
        return data;
    }
}
