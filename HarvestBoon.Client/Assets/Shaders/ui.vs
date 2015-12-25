#version 330 core

layout(location = 0) in vec3 vPosition;

uniform mat4 matModel;
uniform mat4 matScreen;

out vec2 uv;

void main()
{
	uv = vec2(vPosition.x, 1.0f - vPosition.y);
	gl_Position = matScreen * matModel * vec4(vPosition.x, vPosition.y, 0.0f, 1.0f);
}