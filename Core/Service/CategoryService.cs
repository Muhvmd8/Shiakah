using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.Extensions.Localization;
using ServiceAbstraction;
using Shared.Const;
using Shared.DTOs;
using Shared.DTOs.Category;
using Shared.Exceptions;
using Shared.SharedResources;

namespace Service;

public class CategoryService(
    IFileStorageService fileStorageService,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IStringLocalizer<SharedResources> localizer)
    : ICategoryService
{
    public async Task<Result<int>> CreateCategoryAsync(
        CreateCategoryDto createCategoryDto)
    {
        var repo = unitOfWork.GetRepository<Category, int>();

        var existingCategory = await repo.GetFirstOrDefaultAsync(
            c => c.Name.ToLower() ==
                 createCategoryDto.Name.ToLower());

        if (existingCategory != null)
        {
            throw new ConflictException(
                localizer[SharedResourcesKeys.CategoryNameAlreadyExist]);
        }

        var category =
            mapper.Map<Category>(createCategoryDto);

        category.IconUrl =
            await fileStorageService.SaveFileAsync(
                createCategoryDto.Icon,
                CategoryConst.FolderPath,
                CategoryConst.FileMaxSize);

        await repo.AddAsync(category);
        await unitOfWork.SaveChangesAsync();

        return Result<int>.Success(
            category.Id,
            localizer[SharedResourcesKeys.CategoryCreatedSuccessfully]);
    }

    public async Task<Result<int>> UpdateCategoryAsync(
        UpdateCategoryDto updateCategoryDto)
    {
        var repo = unitOfWork.GetRepository<Category, int>();

        var category =
            await repo.GetByIdAsync(updateCategoryDto.Id);

        if (category is null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.CategoryNotFound]);
        }

        var existingCategory =
            await repo.GetFirstOrDefaultAsync(
                c => c.Name.ToLower() ==
                     updateCategoryDto.Name.ToLower() &&
                     c.Id != updateCategoryDto.Id);

        if (existingCategory != null)
        {
            throw new ConflictException(
                localizer[SharedResourcesKeys.CategoryNameAlreadyExist]);
        }

        category.Name = updateCategoryDto.Name;

        if (updateCategoryDto.Icon != null)
        {
            await fileStorageService.DeleteFileAsync(
                category.IconUrl);

            category.IconUrl =
                await fileStorageService.SaveFileAsync(
                    updateCategoryDto.Icon,
                    CategoryConst.FolderPath,
                    CategoryConst.FileMaxSize);
        }

        await unitOfWork.SaveChangesAsync();

        return Result<int>.Success(
            category.Id,
            localizer[SharedResourcesKeys.CategoryUpdatedSuccessfully]);
    }

    public async Task<Result> DeleteCategoryAsync(int id)
    {
        var repo = unitOfWork.GetRepository<Category, int>();

        var category =
            await repo.GetFirstOrDefaultAsync(
                c => c.Id == id);

        if (category is null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.CategoryNotFound]);
        }

        await fileStorageService.DeleteFileAsync(
            category.IconUrl);

        repo.Remove(category);

        await unitOfWork.SaveChangesAsync();

        return Result.Success(
            localizer[SharedResourcesKeys.CategoryDeletedSuccessfully]);
    }

    public async Task<Result<CategoryDto>> GetCategoryByIdAsync(
        int id)
    {
        var repo = unitOfWork.GetRepository<Category, int>();

        var category =
            await repo.GetByIdAsync(id);

        if (category is null)
        {
            throw new NotFoundException(
                localizer[SharedResourcesKeys.CategoryNotFound]);
        }

        var result =
            mapper.Map<CategoryDto>(category);

        return Result<CategoryDto>.Success(
            result,
            localizer[SharedResourcesKeys.CategoryRetrievedSuccessfully]);
    }

    public async Task<Result<IEnumerable<CategoryDto>>>
        GetAllCategoriesAsync()
    {
        var repo = unitOfWork.GetRepository<Category, int>();

        var categories =
            await repo.GetAllAsync();

        var result =
            mapper.Map<IEnumerable<CategoryDto>>(
                categories);

        return Result<IEnumerable<CategoryDto>>.Success(
            result,
            localizer[SharedResourcesKeys.CategoriesRetrievedSuccessfully]);
    }

    public async Task<Result<IEnumerable<CategoryDetailedDto>>>
        GetAllCategoriesForAdminAsync()
    {
        var repo = unitOfWork.GetRepository<Category, int>();

        var categories =
            await repo.GetAllAsync(null, false);

        var result =
            mapper.Map<IEnumerable<CategoryDetailedDto>>(
                categories);

        return Result<IEnumerable<CategoryDetailedDto>>.Success(
            result,
            localizer[SharedResourcesKeys.CategoriesRetrievedSuccessfully]);
    }
}