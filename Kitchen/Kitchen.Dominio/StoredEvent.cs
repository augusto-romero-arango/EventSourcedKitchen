﻿namespace Kitchen.Dominio;

public record StoredEvent(
    Guid AggregateId,
    int SequenceNumber,
    DateTime Timestamp,
    object EventData);