using dominio;
using repositorio.Interfaces;
using repositorio.Contexto;
using service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepositorio usuarioRepositorio;
    private readonly IMapper mapper;

    public UsuarioService(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
    {
        this.usuarioRepositorio = usuarioRepositorio;
        this.mapper = mapper;
    }

    public UsuarioDnit? Obter(UsuarioDnit usuarioDnit)
    {
        UsuarioDnit? usuario = usuarioRepositorio.ObterUsuario(usuarioDnit.email);

        if(usuario == null) throw new KeyNotFoundException();
        
        return usuario;
    }

    public bool validaLogin(UsuarioDTO usuarioDTO)
    {
        var primeiroUsuario = mapper.Map<UsuarioDnit>(usuarioDTO);

        UsuarioDnit segundoUsuario = Obter(primeiroUsuario);
       
        if (validaEmail(primeiroUsuario, segundoUsuario) && validaSenha(primeiroUsuario, segundoUsuario)) return true;
        else return false;
    }

    public bool validaEmail(UsuarioDnit primeiroUsuario, UsuarioDnit segundoUsuario)
    {
        if (segundoUsuario.email == primeiroUsuario.email) return true;
        else return false;
    }

    public bool validaSenha(UsuarioDnit primeiroUsuario, UsuarioDnit segundoUsuario)
    {
        if (segundoUsuario.senha == primeiroUsuario.senha) return true;
        else throw new UnauthorizedAccessException();
    }
}