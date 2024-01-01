using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;

        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;

        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX = 0,
                     translationY = 0,
                     translationZ = 0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0, 0, 0.4f, 1);

            float[] triangleVertices = { 
		        //nose
        0.498F * 30, 0.046F * 30, 0, //0
        0.5F, 0.5F, 0.5F,
        0.525F * 30, 0.376F * 30, 0, //1
        0.5F, 0.5F, 0.5F,
        0.713F * 30, -0.358F * 30, 0, //2
        0.5F, 0.5F, 0.5F,

        0.749F * 30, -0.349F * 30, 0, //3
        0.5F, 0.5F, 0.5F,
        0.704F * 30, -0.349F * 30, 0, //4
        0.5F, 0.5F, 0.5F,
        0.713F * 30, -0.321F * 30, 0, //5
        0.5F, 0.5F, 0.5F,



        //face
        0.498F * 30, 0.046F * 30, 0, //6
        0.5F, 0.5F, 0.5F,
        0.525F * 30, 0.376F * 30, 0, //7
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //8
        0.5F, 0.5F, 0.5F,

        0.525F * 30, 0.376F * 30, 0, //9
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //10
        0.5F, 0.5F, 0.5F,
        0.229F * 30, 0.477F * 30, 0, //11
        0.5F, 0.5F, 0.5F,


        //eyes
        0.372F * 30, 0.266F * 30, 0, //12
        0, 0, 0,
        0.426F * 30, 0.312F * 30, 0, //13
        0F, 0F, 0F,
        0.435F * 30, 0.257F * 30, 0, //14
        0F, 0F, 0F,

        //mouth
        0.372F * 30, 0.266F * 30, 0, //15
        0.5F, 0.5F, 0.5F,
        0.498F * 30, 0.046F * 30, 0, //16
        0.5F, 0.5F, 0.5F,
        0.336F * 30, 0.092F * 30, 0, //17
        0.5F, 0.5F, 0.5F,

        0.336F * 30, 0.092F * 30, 0, //18
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //19
        0.5F, 0.5F, 0.5F,
        0.247F * 30, 0.046F * 30, 0, //20
        0.5F, 0.5F, 0.5F,


        //head
        0.229F * 30, 0.477F * 30, 0, //21
        0.5F, 0.5F, 0.5F,
        0.067F * 30, 0.431F * 30, 0, //22
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //23
        0.5F, 0.5F, 0.5F,

        0.372F * 30, 0.266F * 30, 0, //24
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, 0.376F * 30, 0, //25
        0.5F, 0.5F, 0.5F,
        0.067F * 30, 0.431F * 30, 0, //26
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.376F * 30, 0, //27
        0.5F, 0.5F, 0.5F,
        -0.085F * 30, 0.248F * 30, 0, //28
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //29
        0.5F, 0.5F, 0.5F,

        0.372F * 30, 0.266F * 30, 0, //30
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, 0.165F * 30, 0, //31
        0.5F, 0.5F, 0.5F,
        -0.085F * 30, 0.248F * 30, 0, //32
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.165F * 30, 0, //33
        0.5F, 0.5F, 0.5F,
        0.097F * 30, 0.028F * 30, 0, //34
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //35
        0.5F, 0.5F, 0.5F,

        0.097F * 30, 0.028F * 30, 0, //36
        0.5F, 0.5F, 0.5F,
        0.247F * 30, 0.046F * 30, 0, //37
        0.5F, 0.5F, 0.5F,
        0.372F * 30, 0.266F * 30, 0, //38
        0.5F, 0.5F, 0.5F,

                   //leg1
        -0.013F * 30, -0.055F * 30, 0, //39
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, 0.165F * 30, 0, //40
        0.5F, 0.5F, 0.5F,
        0.097F * 30, 0.028F * 30, 0, //41
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, -0.055F * 30, 0, //42
        0.5F, 0.5F, 0.5F,
        0.097F * 30, 0.028F * 30, 0, //43
        0.5F, 0.5F, 0.5F,
        0.247F * 30, 0.046F * 30, 0, //44
        0.5F, 0.5F, 0.5F,

        0.247F * 30, 0.046F * 30, 0, //45
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, -0.055F * 30, 0, //46
        0.5F, 0.5F, 0.5F,
        0.229F * 30, -0.018F * 30, 0, //47
        0.5F, 0.5F, 0.5F,

        0.229F * 30, -0.018F * 30, 0, //48
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, -0.055F * 30, 0, //49
        0.5F, 0.5F, 0.5F,
        0.085F * 30, -0.22F * 30, 0, //50
        0.5F, 0.5F, 0.5F,

        0.229F * 30, -0.018F * 30, 0, //51
        0.5F, 0.5F, 0.5F,
        0.085F * 30, -0.22F * 30, 0, //52
        0.5F, 0.5F, 0.5F,
        0.121F * 30, -0.303F * 30, 0, //53
        0.5F, 0.5F, 0.5F,

        0.229F * 30, -0.018F * 30, 0, //54
        0.5F, 0.5F, 0.5F,
        0.229F * 30, -0.604F * 30, 0, //55
        0.5F, 0.5F, 0.5F,
        0.121F * 30, -0.303F * 30, 0, //56
        0.5F, 0.5F, 0.5F,


                0.121F * 30, -0.303F * 30, 0, //57
        0.5F, 0.5F, 0.5F,
        0.13F * 30, -0.668F * 30, 0, //58
        0.5F, 0.5F, 0.5F,
        0.229F * 30, -0.604F * 30, 0, //59
        0.5F, 0.5F, 0.5F,

        0.229F * 30, -0.604F * 30, 0, //60
        0.5F, 0.5F, 0.5F,
        0.13F * 30, -0.668F * 30, 0, //61
        0.5F, 0.5F, 0.5F,
        0.22F * 30, -0.659F * 30, 0, //62
        0.5F, 0.5F, 0.5F,

        0.13F * 30, -0.668F * 30, 0, //63
        0.5F, 0.5F, 0.5F,
        0.22F * 30, -0.659F * 30, 0, //64
        0.5F, 0.5F, 0.5F,
        0.121F * 30, -0.668F * 30, 0, //65
        0.5F, 0.5F, 0.5F,

        0.22F * 30, -0.659F * 30, 0, //66
        0.5F, 0.5F, 0.5F,
        0.121F * 30, -0.668F * 30, 0, //67
        0.5F, 0.5F, 0.5F,
        0.256F * 30, -0.705F * 30, 0, //68
        0.5F, 0.5F, 0.5F,

        0.121F * 30, -0.668F * 30, 0, //69
        0.5F, 0.5F, 0.5F,
        0.256F * 30, -0.705F * 30, 0, //70
        0.5F, 0.5F, 0.5F,
        0.121F * 30, -0.714F * 30, 0, //71
        0.5F, 0.5F, 0.5F,

        

         //leg2
        -0.013F * 30, 0.165F * 30, 0, //72
        0.5F, 0.5F, 0.5F,
        -0.211F * 30, 0.018F * 30, 0, //73
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, -0.055F * 30, 0, //74
        0.5F, 0.5F, 0.5F,

        -0.211F * 30, 0.018F * 30, 0, //75
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, -0.055F * 30, 0, //76
        0.5F, 0.5F, 0.5F,
        -0.316F * 30, -0.2F * 30, 0, //77
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, -0.055F * 30, 0, //78
        0.5F, 0.5F, 0.5F,
        -0.316F * 30, -0.2F * 30, 0, //79
        0.5F, 0.5F, 0.5F,
        -0.058F * 30, -0.174F * 30, 0, //80
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, -0.055F * 30, 0, //81
        0.5F, 0.5F, 0.5F,
        -0.058F * 30, -0.174F * 30, 0, //82
        0.5F, 0.5F, 0.5F,
        0.085F * 30, -0.22F * 30, 0, //83
        0.5F, 0.5F, 0.5F,

        -0.058F * 30, -0.174F * 30, 0, //84
        0.5F, 0.5F, 0.5F,
        0.085F * 30, -0.22F * 30, 0, //85
        0.5F, 0.5F, 0.5F,
        -0.085F * 30, -0.6F * 30, 0, //86
        0.5F, 0.5F, 0.5F,

        0.085F * 30, -0.22F * 30, 0, //87
        0.5F, 0.5F, 0.5F,
        -0.085F * 30, -0.6F * 30, 0, //88
        0.5F, 0.5F, 0.5F,
        0.004F * 30, -0.604F * 30, 0, //89
        0.5F, 0.5F, 0.5F,

        -0.085F * 30, -0.6F * 30, 0, //90
        0.5F, 0.5F, 0.5F,
        0.004F * 30, -0.604F * 30, 0, //91
        0.5F, 0.5F, 0.5F,
        -0.121F * 30, -0.668F * 30, 0, //92
        0.5F, 0.5F, 0.5F,

        0.004F * 30, -0.604F * 30, 0, //93
        0.5F, 0.5F, 0.5F,
        -0.121F * 30, -0.668F * 30, 0, //94
        0.5F, 0.5F, 0.5F,
        0.004F * 30, -0.659F * 30, 0, //95
        0.5F, 0.5F, 0.5F,

        -0.121F * 30, -0.668F * 30, 0, //96
        0.5F, 0.5F, 0.5F,
        0.004F * 30, -0.659F * 30, 0, //97
        0.5F, 0.5F, 0.5F,
        -0.121F * 30, -0.714F * 30, 0, //98
        0.5F, 0.5F, 0.5F,

        0.004F * 30, -0.659F * 30, 0, //99
        0.5F, 0.5F, 0.5F,
        -0.121F * 30, -0.714F * 30, 0, //100
        0.5F, 0.5F, 0.5F,
        0.013F * 30, -0.705F * 30, 0, //101
        0.5F, 0.5F, 0.5F,

         


        //leg3,4
        -0.211F * 30, 0.018F * 30, 0, //102
        0.5F, 0.5F, 0.5F,
        -0.316F * 30, -0.2F * 30, 0, //103
        0.5F, 0.5F, 0.5F,
        -0.587F * 30, -0.23F * 30, 0, //104
        0.5F, 0.5F, 0.5F,

        -0.316F * 30, -0.2F * 30, 0, //105
        0.5F, 0.5F, 0.5F,
        -0.587F * 30, -0.23F * 30, 0, //106
        0.5F, 0.5F, 0.5F,
        -0.408F * 30, -0.23F * 30, 0, //107
        0.5F, 0.5F, 0.5F,

        -0.587F * 30, -0.23F * 30, 0, //108
        0.5F, 0.5F, 0.5F,
        -0.408F * 30, -0.23F * 30, 0, //109
        0.5F, 0.5F, 0.5F,
        -0.534F * 30, -0.3F * 30, 0, //110
        0.5F, 0.5F, 0.5F,

        -0.408F * 30, -0.23F * 30, 0, //111
        0.5F, 0.5F, 0.5F,
        -0.534F * 30, -0.3F * 30, 0, //112
        0.5F, 0.5F, 0.5F,
        -0.453F * 30, -0.604F * 30, 0, //113
        0.5F, 0.5F, 0.5F,

         -0.534F * 30, -0.3F * 30, 0, //114
        0.5F, 0.5F, 0.5F,
        -0.453F * 30, -0.604F * 30, 0, //115
        0.5F, 0.5F, 0.5F,
        -0.552F * 30, -0.705F * 30, 0, //116
        0.5F, 0.5F, 0.5F,

        -0.453F * 30, -0.604F * 30, 0, //117
        0.5F, 0.5F, 0.5F,
        -0.552F * 30, -0.705F * 30, 0, //118
        0.5F, 0.5F, 0.5F,
        -0.417F * 30, -0.659F * 30, 0, //119
        0.5F, 0.5F, 0.5F,

        -0.587F * 30, -0.23F * 30, 0, //120
        0.5F, 0.5F, 0.5F,
        -0.534F * 30, -0.3F * 30, 0, //121
        0.5F, 0.5F, 0.5F,
        -0.686F * 30, -0.494F * 30, 0, //122
        0.5F, 0.5F, 0.5F,

        -0.534F * 30, -0.3F * 30, 0, //123
        0.5F, 0.5F, 0.5F,
        -0.686F * 30, -0.494F * 30, 0, //124
        0.5F, 0.5F, 0.5F,
        -0.605F * 30, -0.604F * 30, 0, //125
        0.5F, 0.5F, 0.5F,

        -0.686F * 30, -0.494F * 30, 0, //126
        0.5F, 0.5F, 0.5F,
        -0.605F * 30, -0.604F * 30, 0, //127
        0.5F, 0.5F, 0.5F,
        -0.704F * 30, -0.549F * 30, 0, //128
        0.5F, 0.5F, 0.5F,

        -0.605F * 30, -0.604F * 30, 0, //129
        0.5F, 0.5F, 0.5F,
        -0.704F * 30, -0.549F * 30, 0, //130
        0.5F, 0.5F, 0.5F,
        -0.6F * 30, -0.659F * 30, 0, //131
        0.5F, 0.5F, 0.5F,

        -0.704F * 30, -0.549F * 30, 0, //132
        0.5F, 0.5F, 0.5F,
        -0.6F * 30, -0.659F * 30, 0, //133
        0.5F, 0.5F, 0.5F,
        -0.7F * 30, -0.604F * 30, 0, //134
        0.5F, 0.5F, 0.5F,


        

        //body
        -0.587F * 30, -0.23F * 30, 0, //135
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //136
        0.5F, 0.5F, 0.5F,
        -0.211F * 30, 0.018F * 30, 0, //137
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.376F * 30, 0, //138
        0.5F, 0.5F, 0.5F,
        -0.578F * 30, 0.256F * 30, 0, //139
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //140
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.376F * 30, 0, //141
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //142
        0.5F, 0.5F, 0.5F,
        -0.327F * 30, 0.426F * 30, 0, //143
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.376F * 30, 0, //144
        0.5F, 0.5F, 0.5F,
        -0.327F * 30, 0.426F * 30, 0, //145
        0.5F, 0.5F, 0.5F,
        -0.067F * 30, 0.408F * 30, 0, //146
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.376F * 30, 0, //147
        0.5F, 0.5F, 0.5F,
        -0.067F * 30, 0.408F * 30, 0, //148
        0.5F, 0.5F, 0.5F,
        0.067F * 30, 0.431F * 30, 0, //149
        0.5F, 0.5F, 0.5F,

        -0.013F * 30, 0.376F * 30, 0, //150
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //151
        0.5F, 0.5F, 0.5F,
        -0.316F * 30, -0.2F * 30, 0, //152
        0.5F, 0.5F, 0.5F,

        -0.316F * 30, -0.2F * 30, 0, //153
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, 0.376F * 30, 0, //154
        0.5F, 0.5F, 0.5F,
        -0.013F * 30, 0.165F * 30, 0, //155
        0.5F, 0.5F, 0.5F,



        //tail
        -0.578F * 30, 0.256F * 30, 0, //156
        0.5F, 0.5F, 0.5F,
        -0.659F * 30, 0.13F * 30, 0, //157
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //158
        0.5F, 0.5F, 0.5F,

        -0.659F * 30, 0.13F * 30, 0, //159
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //160
        0.5F, 0.5F, 0.5F,
        -0.659F * 30, 0.004F * 30, 0, //161
        0.5F, 0.5F, 0.5F,

        -0.659F * 30, 0.004F * 30, 0, //162
        0.5F, 0.5F, 0.5F,
        -0.471F * 30, 0.354F * 30, 0, //163
        0.5F, 0.5F, 0.5F,
        -0.632F * 30, -0.004F * 30, 0, //164
        0.5F, 0.5F, 0.5F,

        -0.471F * 30, 0.354F * 30, 0, //165
        0.5F, 0.5F, 0.5F,
        -0.632F * 30, -0.004F * 30, 0, //166
        0.5F, 0.5F, 0.5F,
        -0.587F * 30, -0.23F * 30, 0, //167
        0.5F, 0.5F, 0.5F,

        -0.659F * 30, 0.004F * 30, 0, //168
        0.5F, 0.5F, 0.5F,
        -0.704F * 30, -0.375F * 30, 0, //169
        0.5F, 0.5F, 0.5F,
        -0.632F * 30, -0.004F * 30, 0, //170
        0.5F, 0.5F, 0.5F,


                   -0.704F * 30, -0.375F * 30, 0, //171
        0.5F, 0.5F, 0.5F,
        -0.738F * 30, -0.42F * 30, 0, //172
        0.5F, 0.5F, 0.5F,
        -0.74F * 30, -0.491F * 30, 0, //173
        0.5F, 0.5F, 0.5F,

        -0.74F * 30, -0.491F * 30, 0, //174
        0.5F, 0.5F, 0.5F,
        -0.696F * 30, -0.447F * 30, 0, //175
        0.5F, 0.5F, 0.5F,
        -0.704F * 30, -0.375F * 30, 0, //176
        0.5F, 0.5F, 0.5F,




            ////////////////////////////////////////////////////////////////////////////////////////////////
            //border


        //nose
        0.498F * 30, 0.046F * 30, 0, //177
        0, 0, 0,
        0.525F * 30, 0.376F * 30, 0, //178
        0, 0, 0,
        0.713F * 30, -0.358F * 30, 0, //179
        0, 0, 0,

        0.749F * 30, -0.349F * 30, 0, //180
        0, 0, 0,
        0.704F * 30, -0.349F * 30, 0, //181
        0, 0, 0,
        0.713F * 30, -0.321F * 30, 0, //182
        0, 0, 0,

        //face
        0.498F * 30, 0.046F * 30, 0, //183
        0, 0, 0,
        0.525F * 30, 0.376F * 30, 0, //184
        0, 0, 0,
        0.372F * 30, 0.266F * 30, 0, //185
        0, 0, 0,

        0.525F * 30, 0.376F * 30, 0, //186
        0, 0, 0,
        0.372F * 30, 0.266F * 30, 0, //187
        0, 0, 0,
        0.229F * 30, 0.477F * 30, 0, //188
        0, 0, 0,

        //eyes
        0.372F * 30, 0.266F * 30, 0, //189
        0, 0, 0,
        0.426F * 30, 0.312F * 30, 0, //190
        0, 0, 0,
        0.435F * 30, 0.257F * 30, 0, //191
        0, 0, 0,


        //mouth
        0.372F * 30, 0.266F * 30, 0, //192
        0, 0, 0,
        0.498F * 30, 0.046F * 30, 0, //193
        0, 0, 0,
        0.336F * 30, 0.092F * 30, 0, //194
        0, 0, 0,
        0.247F * 30, 0.046F * 30, 0, //195
        0, 0, 0,

        //head
        0.229F * 30, 0.477F * 30, 0, //196
        0, 0, 0,
        0.067F * 30, 0.431F * 30, 0, //197
        0, 0, 0,
        -0.013F * 30, 0.376F * 30, 0, //198
        0, 0, 0,
        -0.085F * 30, 0.248F * 30, 0, //199
        0, 0, 0,
        -0.013F * 30, 0.165F * 30, 0, //200
        0, 0, 0,
        0.097F * 30, 0.028F * 30, 0, //201
        0, 0, 0,
        0.247F * 30, 0.046F * 30, 0, //202
        0, 0, 0,
        0.372F * 30, 0.266F * 30, 0, //203
        0, 0, 0,

        //leg1
        0.247F * 30, 0.046F * 30, 0, //204
        0, 0, 0,
        0.229F * 30, -0.018F * 30, 0, //205
        0, 0, 0,
        0.229F * 30, -0.604F * 30, 0, //206
        0, 0, 0,
        0.22F * 30, -0.659F * 30, 0, //207
        0, 0, 0,
        0.256F * 30, -0.705F * 30, 0, //208
        0, 0, 0,
        0.121F * 30, -0.714F * 30, 0, //209
        0, 0, 0,
        0.121F * 30, -0.668F * 30, 0, //210
        0, 0, 0,
        0.13F * 30, -0.668F * 30, 0, //211
        0, 0, 0,
        0.121F * 30, -0.303F * 30, 0, //212
        0, 0, 0,
        0.085F * 30, -0.22F * 30, 0, //213
        0, 0, 0,
        -0.013F * 30, -0.055F * 30, 0, //214
        0, 0, 0,
        -0.013F * 30, 0.165F * 30, 0, //215
        0, 0, 0,
        0.097F * 30, 0.028F * 30, 0, //216
        0, 0, 0,

        //leg2
        0.085F * 30, -0.22F * 30, 0, //217
        0, 0, 0,
        0.004F * 30, -0.604F * 30, 0, //218
        0, 0, 0,
        0.004F * 30, -0.659F * 30, 0, //219
        0, 0, 0,
        0.013F * 30, -0.705F * 30, 0, //220
        0, 0, 0,
        -0.121F * 30, -0.714F * 30, 0, //221
        0, 0, 0,
        -0.121F * 30, -0.668F * 30, 0, //222
        0, 0, 0,
        -0.085F * 30, -0.6F * 30, 0, //223
        0, 0, 0,
        -0.058F * 30, -0.174F * 30, 0, //224
        0, 0, 0,

                        //leg3,4
        -0.316F * 30, -0.2F * 30, 0, //225
        0, 0, 0,
        -0.408F * 30, -0.23F * 30, 0, //226
        0, 0, 0,
        -0.453F * 30, -0.604F * 30, 0, //227
        0, 0, 0,
        -0.417F * 30, -0.659F * 30, 0, //228
        0, 0, 0,
        -0.552F * 30, -0.705F * 30, 0, //229
        0, 0, 0,
        -0.534F * 30, -0.3F * 30, 0, //230
        0, 0, 0,
        -0.605F * 30, -0.604F * 30, 0, //231
        0, 0, 0,
        -0.6F * 30, -0.659F * 30, 0, //232
        0, 0, 0,
        -0.7F * 30, -0.604F * 30, 0, //233
        0, 0, 0,
        -0.704F * 30, -0.549F * 30, 0, //234
        0, 0, 0,
        -0.686F * 30, -0.494F * 30, 0, //235
        0, 0, 0,
        -0.587F * 30, -0.23F * 30, 0, //236
        0, 0, 0,

        //tail
        -0.632F * 30, -0.004F * 30, 0, //237
        0, 0, 0,
        -0.704F * 30, -0.375F * 30, 0, //238
        0, 0, 0,
        -0.696F * 30, -0.447F * 30, 0, //239
        0, 0, 0,
        -0.74F * 30, -0.491F * 30, 0, //240
        0, 0, 0,
        -0.738F * 30, -0.42F * 30, 0, //241
        0, 0, 0,
        -0.704F * 30, -0.375F * 30, 0, //242
        0, 0, 0,
        -0.659F * 30, 0.004F * 30, 0, //243
        0, 0, 0,
        -0.659F * 30, 0.13F * 30, 0, //244
        0, 0, 0,
        -0.578F * 30, 0.256F * 30, 0, //245
        0, 0, 0,
        -0.471F * 30, 0.354F * 30, 0, //246
        0, 0, 0,
        -0.327F * 30, 0.426F * 30, 0, //247
        0, 0, 0,
        -0.067F * 30, 0.408F * 30, 0, //248
        0, 0, 0,
        0.067F * 30, 0.431F * 30, 0, //249
        0, 0, 0,

        -0.013F * 30, 0.376F * 30, 0, //250
        0, 0, 0,
        -0.085F * 30, 0.248F * 30, 0, //251
        0, 0, 0,
        -0.013F * 30, 0.165F * 30, 0, //252
        0, 0, 0,
        -0.013F * 30, -0.055F * 30, 0, //253
        0, 0, 0,
            }; // Triangle Center = (10, 7, -5)

            triangleCenter = new vec3(2, 2, 0);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f,
                0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 100.0f,
                0.0f, 0.0f, 1.0f,
            };


            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);

            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated Triangle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);//nose
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 6, 6);//face
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 12, 3);//eyes
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 15, 6);//mouth
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 21, 18);//head

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 39, 33);//leg1
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 72, 30);//leg2
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 102, 33);//leg3,4

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 135, 21);//body
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 156, 21);//tail




            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 177, 3);//nose
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 180, 3);//nose
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 183, 3);//face
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 186, 3);//face
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 189, 3);//eyes
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 192, 4);//mouth
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 196, 8);//head
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 204, 13);//leg1
            Gl.glDrawArrays(Gl.GL_LINE_LOOP, 217, 37);//leg2,3,4 body tail


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }


        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds / 1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1), triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix = MathHelper.MultiplyMatrices(transformations);

            timer.Reset();
            timer.Start();
        }

        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
