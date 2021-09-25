export class Pagination {
  page: number;
  limit: number;
  totalItems: number;
  totalPages: number;
}

export class Paginated<T> extends Pagination {
  items: T[];
}
