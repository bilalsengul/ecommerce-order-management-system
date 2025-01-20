import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm, useFieldArray } from 'react-hook-form';
import { ordersApi } from '../api/client';
import { CreateOrderDto, CreateOrderItemDto } from '../api/types';

interface OrderForm {
  userId: string;
  items: {
    productId: string;
    quantity: number;
    price: number;
  }[];
}

export default function CreateOrderPage() {
  const navigate = useNavigate();
  const [isSubmitting, setIsSubmitting] = useState(false);

  const {
    register,
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<OrderForm>({
    defaultValues: {
      userId: '',
      items: [{ productId: '', quantity: 1, price: 0 }],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: 'items',
  });

  const onSubmit = async (data: OrderForm) => {
    try {
      setIsSubmitting(true);
      const orderData: CreateOrderDto = {
        userId: data.userId,
        items: data.items.map((item): CreateOrderItemDto => ({
          productId: item.productId,
          quantity: item.quantity,
          price: item.price,
        })),
      };

      await ordersApi.createOrder(orderData);
      navigate('/orders');
    } catch (error) {
      console.error('Failed to create order:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="bg-white shadow sm:rounded-lg">
      <div className="px-4 py-5 sm:p-6">
        <div className="sm:flex sm:items-center">
          <div className="sm:flex-auto">
            <h1 className="text-base font-semibold leading-6 text-gray-900">Create New Order</h1>
            <p className="mt-2 text-sm text-gray-700">
              Fill in the details below to create a new order.
            </p>
          </div>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="mt-6 space-y-6">
          <div>
            <label htmlFor="userId" className="block text-sm font-medium text-gray-700">
              User ID
            </label>
            <input
              type="text"
              id="userId"
              {...register('userId', { required: 'User ID is required' })}
              className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
            />
            {errors.userId && (
              <p className="mt-2 text-sm text-red-600">{errors.userId.message}</p>
            )}
          </div>

          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <h2 className="text-sm font-medium text-gray-900">Order Items</h2>
              <button
                type="button"
                onClick={() => append({ productId: '', quantity: 1, price: 0 })}
                className="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500"
              >
                Add Item
              </button>
            </div>

            {fields.map((field, index) => (
              <div key={field.id} className="rounded-lg bg-gray-50 p-4">
                <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
                  <div>
                    <label
                      htmlFor={`items.${index}.productId`}
                      className="block text-sm font-medium text-gray-700"
                    >
                      Product ID
                    </label>
                    <input
                      type="text"
                      {...register(`items.${index}.productId` as const, {
                        required: 'Product ID is required',
                      })}
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                    />
                    {errors.items?.[index]?.productId && (
                      <p className="mt-2 text-sm text-red-600">
                        {errors.items[index]?.productId?.message}
                      </p>
                    )}
                  </div>

                  <div>
                    <label
                      htmlFor={`items.${index}.quantity`}
                      className="block text-sm font-medium text-gray-700"
                    >
                      Quantity
                    </label>
                    <input
                      type="number"
                      min="1"
                      {...register(`items.${index}.quantity` as const, {
                        required: 'Quantity is required',
                        min: { value: 1, message: 'Quantity must be at least 1' },
                      })}
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                    />
                    {errors.items?.[index]?.quantity && (
                      <p className="mt-2 text-sm text-red-600">
                        {errors.items[index]?.quantity?.message}
                      </p>
                    )}
                  </div>

                  <div>
                    <label
                      htmlFor={`items.${index}.price`}
                      className="block text-sm font-medium text-gray-700"
                    >
                      Price
                    </label>
                    <input
                      type="number"
                      step="0.01"
                      min="0"
                      {...register(`items.${index}.price` as const, {
                        required: 'Price is required',
                        min: { value: 0, message: 'Price must be at least 0' },
                      })}
                      className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                    />
                    {errors.items?.[index]?.price && (
                      <p className="mt-2 text-sm text-red-600">
                        {errors.items[index]?.price?.message}
                      </p>
                    )}
                  </div>
                </div>

                {fields.length > 1 && (
                  <button
                    type="button"
                    onClick={() => remove(index)}
                    className="mt-2 text-sm text-red-600 hover:text-red-900"
                  >
                    Remove Item
                  </button>
                )}
              </div>
            ))}
          </div>

          <div className="flex justify-end">
            <button
              type="submit"
              disabled={isSubmitting}
              className="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
            >
              {isSubmitting ? 'Creating Order...' : 'Create Order'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
} 