using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;


// [       HEADER           ]
// [Protocol ] [ bodyLength ] [ BODY ]
public class NetPacket
{
    public int protocol;  
    public int bodyLength;  
    public byte[] packetData;  // [Protocol ] [ bodyLength ] [ BODY ]

    public NetPacket() { }

    public NetPacket(int protocol) 
    {
        this.protocol = protocol;

        MakePacketData();
    }

    // SOCKET : only byte[] 
    // StreamWriter || StreamReader : string -> byte[] -> string
    public NetPacket(int protocol, string data)
    {
        this.protocol = protocol;
        byte[] body = StringToByteArray(data);
        
        //    packetData = [  빈 값                  ]
        //    sourceByteArray : 전체 [ 0~Length ]
        //    => 
        //    destinationByteArray : protocolHeader 사이즈만큼만 [ 0 ] 에서 부터
        // =  packetData = [ protocolHeader ] [ 빈 값 ]

        // protocolHeader Length : 4 byte
        //               protocol    length      data
        // packetData = '[][][][]' '[][][][]' [][][][][][]~

        MakePacketData(body);
    }

    public NetPacket(int protocol, object obj)
    {
        this.protocol = protocol;
        byte[] body = ObjectToByteArray(obj);
     
        MakePacketData(body);
    }

    public NetPacket(int protocol, int data, int listOrder)
    {
        this.protocol = protocol;

        string[] body = new string[2];
        body[0] = data.ToString();
        body[1] = listOrder.ToString();

        MakePacketData(ObjectToByteArray(body));
    }

    private void MakePacketData()
    {
        // int to byteArray
        byte[] protocolHeader = BitConverter.GetBytes(protocol);

        // Length of Body ByteArray
        int bodyLength = 0;
        byte[] bodyLengthHeader = BitConverter.GetBytes(bodyLength);

        packetData = new byte[protocolHeader.Length + bodyLengthHeader.Length];

        Array.Copy(protocolHeader, 0, packetData, 0, protocolHeader.Length);
        Array.Copy(bodyLengthHeader, 0, packetData, protocolHeader.Length, bodyLengthHeader.Length);
    }

    private void MakePacketData(byte[] body)
    {
        // int to byteArray
        byte[] protocolHeader = BitConverter.GetBytes(protocol);

        // Length of Body ByteArray
        int bodyLength = body.Length;
        byte[] bodyLengthHeader = BitConverter.GetBytes(bodyLength);

        packetData = new byte[protocolHeader.Length + bodyLengthHeader.Length + body.Length];

        Array.Copy(protocolHeader, 0, packetData, 0, protocolHeader.Length);
        Array.Copy(bodyLengthHeader, 0, packetData, protocolHeader.Length, bodyLengthHeader.Length);
        Array.Copy(body, 0, packetData, protocolHeader.Length + bodyLengthHeader.Length, body.Length);
    }

    // byte[] => Packet
    public NetPacket(byte[] bytes)
    {
        packetData = bytes;
        // byte[] : 0~3  =>  int
        protocol = BitConverter.ToInt32(bytes, 0);
        bodyLength = BitConverter.ToInt32(bytes, 4);
    }
   
    //             length: 4+4      bodyLength
    // packetData : [ Header   ]    8: [ Body  ]
    //             bodyLength
    // body       : [ Body ]
    public string PopString()
    {
        byte[] body = new byte[bodyLength];
        Array.Copy(packetData, 8, body,0, bodyLength);
        return ByteArrayToString(body);
    }

    internal int PopInt()
    {
        return BitConverter.ToInt32(packetData, 8);
    }

    private byte[] StringToByteArray(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    private string ByteArrayToString(byte[] bytes)
    {
        return Encoding.Default.GetString(bytes);
    }

    // Object To ByteArray
    private byte[] ObjectToByteArray(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);
        return ms.ToArray();
    }

    public object PopObject()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        ms.Write(packetData, 8, bodyLength);
        ms.Seek(0, SeekOrigin.Begin);
        return bf.Deserialize(ms);
    }
}
