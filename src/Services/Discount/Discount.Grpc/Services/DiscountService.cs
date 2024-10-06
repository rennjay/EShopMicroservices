using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountDbContext discountDb, ILogger<DiscountService> logger) 
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountDb.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProductName ==  request.ProductName);

        if (coupon == null)
        {
            logger.LogWarning("Product discount not found. ProductName: {ProductName}", request.ProductName);
            throw new RpcException(new Status(StatusCode.NotFound, $"Product discount not found. productName: {request.ProductName}"));
        }

        logger.LogInformation("Product Coupon is successfully retrieved. Discount: {@Coupon}", JsonSerializer.Serialize<Coupon>(coupon));
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var newCoupon = request.Coupon.Adapt<Coupon>();
        if (newCoupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid coupon data"));

        discountDb.Coupons.Add(newCoupon);
        await discountDb.SaveChangesAsync();

        logger.LogInformation("New coupon is successfully created. ProductName: {name}", newCoupon.ProductName);
        return newCoupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var isCouponExists = discountDb.Coupons.Any(c => c.ProductName == request.Coupon.ProductName);
        if (!isCouponExists)
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not found. ProductName: {request.Coupon.ProductName}"));

        var updatedCoupon = request.Coupon.Adapt<Coupon>();
        if (updatedCoupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        discountDb.Coupons.Update(updatedCoupon);
        await discountDb.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully updated. ProductName: {name}", updatedCoupon.ProductName);
        return updatedCoupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = discountDb.Coupons.FirstOrDefault(c => c.ProductName == request.ProductName);
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Coupon not found. ProductName: {request.ProductName}"));

        discountDb.Remove(coupon);
        await discountDb.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully deleted. ProductName: {name}", coupon.ProductName);

        return new DeleteDiscountResponse()
        {
            Success = true,
        };
    }
}
