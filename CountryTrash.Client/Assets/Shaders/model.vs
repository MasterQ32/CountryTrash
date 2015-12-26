#version 330 core

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec3 vNormal;
layout(location = 2) in vec2 vUV;

uniform mat4 matModel;
uniform mat4 matViewProj;

out vec2 uv;
out vec3 normal;

void main()
{
	uv = vUV;
	normal = (matModel * vec4(vNormal, 0.0f)).xyz;
	gl_Position = matViewProj * matModel * vec4(vPosition, 1.0f);
}