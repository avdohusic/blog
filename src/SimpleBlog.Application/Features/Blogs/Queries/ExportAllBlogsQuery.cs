using AutoMapper;
using ClosedXML.Excel;
using SimpleBlog.Application.Extensions;
using SimpleBlog.Domain.Constants;
using SimpleBlog.Domain.Repositories;
using System.Data;

namespace SimpleBlog.Application.Features.Blogs.Queries;
public sealed record ExportAllBlogsQuery : IQuery<MemoryStream>;

public class ExportAllBlogsQueryHandler : IQueryHandler<ExportAllBlogsQuery, MemoryStream>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public ExportAllBlogsQueryHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<MemoryStream> Handle(ExportAllBlogsQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.GetAllBlogs();
        var mappedBlogs = _mapper.Map<IEnumerable<BlogExcelDto>>(blogs);

        // Create the workbook
        XLWorkbook workbook = new XLWorkbook();

        DataSet blogsDataSet = PrepareBlogDataSet(mappedBlogs);
        workbook.AddWorksheet(blogsDataSet);

        // prepare the file
        MemoryStream excelStream = new();
        workbook.SaveAs(excelStream);
        excelStream.Position = 0;

        return excelStream;
    }

    private DataSet PrepareBlogDataSet(IEnumerable<BlogExcelDto> blogs)
    {
        var dataSet = new DataSet();

        var blogDataTable = blogs.ToDataTable(tableName: GlobalConstants.BlogTableName);

        dataSet.Tables.Add(blogDataTable);

        return dataSet;
    }
}