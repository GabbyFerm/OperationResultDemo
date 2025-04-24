using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();           
            CreateMap<CreateBookCommand, Book>();
            CreateMap<UpdateBookCommand, Book>();
            CreateMap<DeleteBookCommand, Book>();

            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<CreateAuthorCommand, Author>();
            CreateMap<UpdateAuthorCommand, Author>();
            CreateMap<DeleteAuthorCommand, Author>();
        }
    }
}
